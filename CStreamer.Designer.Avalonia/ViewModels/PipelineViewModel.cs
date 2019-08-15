// -----------------------------------------------------------------------
// <copyright file="PipelineViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using CStreamer.Base;
    using CStreamer.Base.BaseElements;
    using CStreamer.Designer.Avalonia.Helper;
    using CStreamer.Events;
    using global::Avalonia;
    using ReactiveUI;

    internal class PipelineViewModel : ViewModelBase
    {
        private readonly PipeLine pipeline;

        private readonly Dictionary<IElement, ElementViewModel> elementLookup;

        private readonly Dictionary<ISinkPad, PadViewModel> sinkPadLookup;

        private readonly Dictionary<ISrcPad, PadViewModel> srcPadLookup;

        private readonly Dictionary<(ISrcPad, ISinkPad), LinkViewModel> linkLookup;

        private readonly Dictionary<string, Point> positionCache;

        private IElement? selectedElement;

        public PipelineViewModel(PipeLine pipeline, Action<Avalonia.Notification> addNotification)
        {
            this.pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            this.Items = new ObservableCollection<ICStreamerViewModel>();
            this.elementLookup = new Dictionary<IElement, ElementViewModel>();
            this.srcPadLookup = new Dictionary<ISrcPad, PadViewModel>();
            this.sinkPadLookup = new Dictionary<ISinkPad, PadViewModel>();
            this.linkLookup = new Dictionary<(ISrcPad, ISinkPad), LinkViewModel>();
            this.positionCache = new Dictionary<string, Point>();

            this.pipeline.ElementAdded += this.Pipeline_ElementAdded;
            this.pipeline.ElementRemoved += this.Pipeline_ElementRemoved;
            this.pipeline.ElementsLinked += this.Pipeline_ElementsLinked;
            this.pipeline.ElementsUnlinked += this.Pipeline_ElementsUnlinked;

            this.WhenActivated((disposables) =>
            {
                var errors = Observable.FromEventPattern<ErrorEventArgs>((handler) => this.pipeline.Error += handler, (handler) => this.pipeline.Error -= handler);

                errors.Subscribe(error => addNotification(new Avalonia.Notification(error.EventArgs.Text))).DisposeWith(disposables);
            });
        }

        public ObservableCollection<ICStreamerViewModel> Items { get; }

        public IElement? SelectedElement
        {
            get => this.selectedElement;
            set => this.RaiseAndSetIfChanged(ref this.selectedElement, value);
        }

        /// <summary>
        /// Removes an element from the pipeline.
        /// This unlinks the element from all other elements.
        /// Afterwars this element won't be handled by the pipleline.
        /// </summary>
        /// <param name="graphicalElement">The element to remove from the pipeline.</param>
        public void Remove(ElementViewModel graphicalElement)
        {
            if (graphicalElement == null)
            {
                throw new ArgumentNullException(nameof(graphicalElement));
            }

            this.pipeline.Remove(graphicalElement.Model);
        }

        /// <summary>
        /// Add an element to the pipeline to be handled.
        /// The pipeline handles the <see cref="State"/> of all it's elements and serielizes elements it owns when it gets serialized.
        /// </summary>
        /// <param name="element">The element to add to the pipeline.</param>
        /// <param name="position">The position the element should be rendered at.</param>
        public void Add(IElement element, Point position)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            this.positionCache.Add(element.Name, position);
            this.pipeline.Add(element);
        }

        /// <summary>
        /// Connect two element pads, letting them send data from the src to the sink.
        /// </summary>
        /// <typeparam name="TValue">The type of data that can be sent.</typeparam>
        /// <param name="src">The src ot the data.</param>
        /// <param name="sink">The destination of the data.</param>
        public void Connect<TValue>(ISrcPad<TValue> src, ISinkPad<TValue> sink)
        {
            this.pipeline.Connect<TValue>(src, sink);
        }

        /// <summary>
        /// Moves the pipeline and all it's elements to the <see cref="State.Playing"/> state.
        /// </summary>
        /// <returns>A task that represents the state change operation.</returns>
        public Task Start()
        {
            return this.pipeline.Start();
        }

        /// <summary>
        /// Moves the pipeline and all it's elements to the <see cref="State.Stopped"/> state.
        /// </summary>
        /// <returns>A task that represents the state change operation.</returns>
        public Task Stop()
        {
            return this.pipeline.Stop();
        }

        internal IList<string> LoadPipeline(DesignerPipelineDefinition definition)
        {
            foreach (var kvp in definition.Positions)
            {
                this.positionCache.Add(kvp.Key, kvp.Value);
            }

            return this.pipeline.FromDefinition(definition);
        }

        internal DesignerPipelineDefinition SavePipeline()
        {
            var rawDefinition = this.pipeline.GetDefinition();

            var positions = this.elementLookup.ToDictionary(e => e.Key.Name, e => new Point(e.Value.X, e.Value.Y));

            var graphicalDefinition = new DesignerPipelineDefinition(rawDefinition, positions);

            return graphicalDefinition;
        }

        internal void TryConnect(SrcPadViewModel src, SinkPadViewModel sink)
        {
            this.pipeline.TryConnect(src.Model, sink.Model);
        }

        internal void CreateElement(string name, Point position)
        {
            var element = PipeElementFactory.Make(name, null);
            if (element != null)
            {
                this.Add(element, position);
            }
        }

        private void Pipeline_ElementsUnlinked(object? sender, ElementsUnlinkedEventArgs e)
        {
            if (this.linkLookup.Remove((e.Src, e.Sink), out LinkViewModel? link))
            {
                this.Items.Remove(link);
            }
        }

        private void Pipeline_ElementsLinked(object? sender, ElementsLinkedEventArgs e)
        {
            var index = (e.Src, e.Sink);
            if (this.linkLookup.ContainsKey(index))
            {
                return;
            }

            if (this.srcPadLookup.TryGetValue(e.Src, out PadViewModel? src) &&
                this.sinkPadLookup.TryGetValue(e.Sink, out PadViewModel? sink))
            {
                LinkViewModel link = new LinkViewModel(src, sink);
                this.linkLookup.Add(index, link);
                this.Items.Add(link);
            }
        }

        private void Pipeline_ElementRemoved(object? sender, ElementRemovedEventArgs e)
        {
            if (this.elementLookup.Remove(e.Element, out ElementViewModel? element))
            {
                this.Items.Remove(element);
            }

            foreach (var sink in e.Element.GetSinkPads())
            {
                if (this.sinkPadLookup.Remove(sink, out PadViewModel? sinkPadViewModel))
                {
                    this.Items.Remove(sinkPadViewModel);
                }
            }

            foreach (var src in e.Element.GetSrcPads())
            {
                if (this.srcPadLookup.Remove(src, out PadViewModel? srcPadViewModel))
                {
                    this.Items.Remove(srcPadViewModel);
                }
            }
        }

        private void Pipeline_ElementAdded(object? sender, ElementAddedEventArgs e)
        {
            ElementViewModel element;
            if (this.positionCache.Remove(e.Element.Name, out Point position))
            {
                element = new ElementViewModel(e.Element, position);
            }
            else
            {
                element = new ElementViewModel(e.Element, default);
            }

            this.elementLookup.Add(e.Element, element);
            this.Items.Add(element);

            int padNr = 0;
            foreach (var sink in e.Element.GetSinkPads())
            {
                var graphicalSinkPad = new SinkPadViewModel(sink, element, padNr++);
                this.sinkPadLookup.Add(sink, graphicalSinkPad);
                this.Items.Add(graphicalSinkPad);
            }

            padNr = 0;
            foreach (var src in e.Element.GetSrcPads())
            {
                var graphicalSrcPad = new SrcPadViewModel(src, element, padNr++);
                this.srcPadLookup.Add(src, graphicalSrcPad);
                this.Items.Add(graphicalSrcPad);
            }
        }
    }
}
