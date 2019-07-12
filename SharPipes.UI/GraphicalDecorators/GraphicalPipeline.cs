using SharPipes.Pipes.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;
using SharPipes.Pipes.Base.Events;
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.UI.GraphicalDecorators
{
    public class GraphicalPipeline : INotifyCollectionChanged, IEnumerable<IGraphical>
    {
        private readonly PipeLine pipeline;
        private readonly ObservableCollection<IGraphical> graphicals;
        private readonly Dictionary<IPipeElement, GraphicalElement> elementLookup;
        private readonly Dictionary<IPipeSinkPad, GraphicalSinkPad> sinkPadLookup;
        private readonly Dictionary<IPipeSrcPad, GraphicalSrcPad> srcPadLookup;
        private readonly Dictionary<(IPipeSrcPad, IPipeSinkPad), GraphicalEdge> linkLookup;

        private readonly Dictionary<IPipeElement, Point> positionCache;

        public void Remove(GraphicalElement graphicalElement)
        {
            pipeline.Remove(graphicalElement.Element);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public GraphicalPipeline(PipeLine pipeLine)
        {
            this.pipeline = pipeLine;
            this.graphicals = new ObservableCollection<IGraphical>();
            this.elementLookup = new Dictionary<IPipeElement, GraphicalElement>();
            this.srcPadLookup = new Dictionary<IPipeSrcPad, GraphicalSrcPad>();
            this.sinkPadLookup = new Dictionary<IPipeSinkPad, GraphicalSinkPad>();
            this.linkLookup = new Dictionary<(IPipeSrcPad, IPipeSinkPad), GraphicalEdge>();
            this.positionCache = new Dictionary<IPipeElement, Point>();

            this.pipeline.ElementAdded += Pipeline_ElementAdded;
            this.pipeline.ElementRemoved += Pipeline_ElementRemoved;
            this.pipeline.ElementsLinked += Pipeline_ElementsLinked;
            this.pipeline.ElementsUnlinked += Pipeline_ElementsUnlinked;
            this.graphicals.CollectionChanged += Graphicals_CollectionChanged;
        }

        private void Graphicals_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
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
                if(this.sinkPadLookup.Remove(sink, out GraphicalSinkPad gSink))
                {
                    this.graphicals.Remove(gSink);
                }
            }

            foreach (var src in e.Element.GetSrcPads())
            {
                if (this.srcPadLookup.Remove(src, out GraphicalSrcPad gSrc))
                {
                    this.graphicals.Remove(gSrc);
                }
            }

            this.positionCache.Remove(e.Element);
        }

        private void Pipeline_ElementAdded(object sender, ElementAddedEventArgs e)
        {
            GraphicalElement element;
            if (this.positionCache.TryGetValue(e.Element, out Point position))
            {
                element = new GraphicalElement(e.Element, position, this);
            }
            else
            {
                element = new GraphicalElement(e.Element, new Point(), this);
            }

            int padNr = 0;
            foreach (var sink in e.Element.GetSinkPads())
            {
                var gSink = new GraphicalSinkPad(sink, element, padNr);
                this.sinkPadLookup.Add(sink, gSink);
                this.graphicals.Add(gSink);
            }

            padNr = 0;
            foreach (var src in e.Element.GetSrcPads())
            {
                var gSrc = new GraphicalSrcPad(src, element, padNr);
                this.srcPadLookup.Add(src, gSrc);
                this.graphicals.Add(gSrc);
            }

            this.elementLookup.Add(e.Element, element);
            this.graphicals.Add(element);
        }

        public IPipeElement CreateNodeFromTemplate(IPipeElement template, Point position)
        {
            var node = this.pipeline.CreateNodeFromTemplate(template);
            this.Add(node, position);
            return node;
        }



        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged.Invoke(this, args);
            }
        }

        public void Add(IPipeElement node, Point position)
        {
            this.positionCache.Add(node, position);
            this.pipeline.Add(node);
        }

        public void Connect<TValue>(PipeSrcPad<TValue> From, PipeSinkPad<TValue> To)
        {
            this.pipeline.Connect<TValue>(From, To);
        }

        public Task Start()
        {
            return this.pipeline.Start();
        }

        public Task Stop()
        {
            return this.pipeline.Stop();
        }

        internal void TryConnect(GraphicalSrcPad src, GraphicalSinkPad sink)
        {
            this.pipeline.TryConnect(src.Element, sink.Element);
        }

        public IEnumerator<IGraphical> GetEnumerator()
        {
            return graphicals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal GraphicalPipeLineDefinition GetDefinition()
        {
            var rawDefinition = pipeLine.GetDefinition();
            var elementLookup = this.nodes.ToDictionary(elem => elem.Element.Name);

            var graphicalDefinition = new GraphicalPipeLineDefinition(rawDefinition.Links);

            foreach(ElementDefinition element in rawDefinition.Elements)
            {
                var gElement = elementLookup[element.Name];
                graphicalDefinition.Elements.Add(new GraphicalElementDefinition(element, gElement.X, gElement.Y));
            }


            return graphicalDefinition;
        }

        internal IList<string> FromDefinition(GraphicalPipeLineDefinition definition)
        {
            return pipeLine.FromDefinition(definition);
        }
    }
}
