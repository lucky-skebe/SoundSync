// -----------------------------------------------------------------------
// <copyright file="GraphicalPipeline.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using SharPipes.Pipes.Base;
    using SharPipes.Pipes.Base.Events;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    /// <summary>
    /// A <see cref="PipeLine"/> with additional position information.
    ///
    /// All calls to this pipeline will be forwarded to the underlying pipeline and this pipeline reacts to the different pipeline events.
    /// </summary>
    public class GraphicalPipeline : INotifyCollectionChanged, IEnumerable<IGraphical>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        private readonly PipeLine pipeline;
        private readonly ObservableCollection<IGraphical> graphicals;
        private readonly Dictionary<IElement, GraphicalElement> elementLookup;
        private readonly Dictionary<ISinkPad, GraphicalSinkPad> sinkPadLookup;
        private readonly Dictionary<ISrcPad, GraphicalSrcPad> srcPadLookup;
        private readonly Dictionary<(ISrcPad, ISinkPad), GraphicalEdge> linkLookup;

        private readonly Dictionary<string, Point> positionCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicalPipeline"/> class.
        /// </summary>
        /// <param name="pipeLine">the underlying piple to forward calls to.</param>
        public GraphicalPipeline(PipeLine pipeLine)
        {
            this.pipeline = pipeLine ?? throw new ArgumentNullException(nameof(pipeLine));
            this.graphicals = new ObservableCollection<IGraphical>();
            this.elementLookup = new Dictionary<IElement, GraphicalElement>();
            this.srcPadLookup = new Dictionary<ISrcPad, GraphicalSrcPad>();
            this.sinkPadLookup = new Dictionary<ISinkPad, GraphicalSinkPad>();
            this.linkLookup = new Dictionary<(ISrcPad, ISinkPad), GraphicalEdge>();
            this.positionCache = new Dictionary<string, Point>();

            this.pipeline.ElementAdded += this.Pipeline_ElementAdded;
            this.pipeline.ElementRemoved += this.Pipeline_ElementRemoved;
            this.pipeline.ElementsLinked += this.Pipeline_ElementsLinked;
            this.pipeline.ElementsUnlinked += this.Pipeline_ElementsUnlinked;
            this.graphicals.CollectionChanged += this.Graphicals_CollectionChanged;
        }

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Removes an element from the pipeline.
        /// This unlinks the element from all other elements.
        /// Afterwars this element won't be handled by the pipleline.
        /// </summary>
        /// <param name="graphicalElement">The element to remove from the pipeline.</param>
        public void Remove(GraphicalElement graphicalElement)
        {
            if (graphicalElement == null)
            {
                throw new ArgumentNullException(nameof(graphicalElement));
            }

            this.pipeline.Remove(graphicalElement.Element);
        }

        /// <summary>
        /// Creates a new element from an existing template.
        /// Only the type gets copied.
        /// Properties need to be initialized manually.
        /// </summary>
        /// <param name="template">The template element.</param>
        /// <param name="position">The position the element should be rendered at.</param>
        /// <returns>A new element of the same type as the template.</returns>
        public IElement CreateNodeFromTemplate(IElement template, Point position)
        {
            var node = PipeLine.CreateNodeFromTemplate(template);
            this.Add(node, position);
            return node;
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
        public void Connect<TValue>(SrcPad<TValue> src, SinkPad<TValue> sink)
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

        /// <inheritdoc/>
        public IEnumerator<IGraphical> GetEnumerator()
        {
            return this.graphicals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal void TryConnect(GraphicalSrcPad src, GraphicalSinkPad sink)
        {
            this.pipeline.TryConnect(src.Element, sink.Element);
        }

        internal GraphicalPipeLineDefinition GetDefinition()
        {
            var rawDefinition = this.pipeline.GetDefinition();

            var positions = this.elementLookup.ToDictionary(e => e.Key.Name, e => new Point(e.Value.X, e.Value.Y));

            var graphicalDefinition = new GraphicalPipeLineDefinition(rawDefinition, positions);

            return graphicalDefinition;
        }

        internal IList<string> FromDefinition(GraphicalPipeLineDefinition definition)
        {
            foreach (var kvp in definition.Positions)
            {
                this.positionCache.Add(kvp.Key, kvp.Value);
            }

            return this.pipeline.FromDefinition(definition);
        }

        private void Graphicals_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        private void Pipeline_ElementsUnlinked(object sender, ElementsUnlinkedEventArgs e)
        {
            if (this.linkLookup.Remove((e.Src, e.Sink), out GraphicalEdge link))
            {
                this.graphicals.Remove(link);
            }
        }

        private void Pipeline_ElementsLinked(object sender, ElementsLinkedEventArgs e)
        {
            if (this.srcPadLookup.TryGetValue(e.Src, out GraphicalSrcPad src) &&
                this.sinkPadLookup.TryGetValue(e.Sink, out GraphicalSinkPad sink))
            {
                var index = (e.Src, e.Sink);
                GraphicalEdge link = new GraphicalEdge(src, sink);
                this.linkLookup.Add(index, link);
                this.graphicals.Add(link);
            }
        }

        private void Pipeline_ElementRemoved(object sender, ElementRemovedEventArgs e)
        {
            if (this.elementLookup.Remove(e.Element, out GraphicalElement element))
            {
                this.graphicals.Remove(element);
            }

            foreach (var sink in e.Element.GetSinkPads())
            {
                if (this.sinkPadLookup.Remove(sink, out GraphicalSinkPad graphicalSinkPad))
                {
                    this.graphicals.Remove(graphicalSinkPad);
                }
            }

            foreach (var src in e.Element.GetSrcPads())
            {
                if (this.srcPadLookup.Remove(src, out GraphicalSrcPad graphicalSrcPad))
                {
                    this.graphicals.Remove(graphicalSrcPad);
                }
            }
        }

        private void Pipeline_ElementAdded(object sender, ElementAddedEventArgs e)
        {
            GraphicalElement element;
            if (this.positionCache.Remove(e.Element.Name, out Point position))
            {
                element = new GraphicalElement(e.Element, position, this);
            }
            else
            {
                element = new GraphicalElement(e.Element, default(Point), this);
            }

            int padNr = 0;
            foreach (var sink in e.Element.GetSinkPads())
            {
                var graphicalSinkPad = new GraphicalSinkPad(sink, element, padNr);
                this.sinkPadLookup.Add(sink, graphicalSinkPad);
                this.graphicals.Add(graphicalSinkPad);
            }

            padNr = 0;
            foreach (var src in e.Element.GetSrcPads())
            {
                var graphicalSrcPad = new GraphicalSrcPad(src, element, padNr);
                this.srcPadLookup.Add(src, graphicalSrcPad);
                this.graphicals.Add(graphicalSrcPad);
            }

            this.elementLookup.Add(e.Element, element);
            this.graphicals.Add(element);
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged.Invoke(this, args);
            }
        }
    }
}
