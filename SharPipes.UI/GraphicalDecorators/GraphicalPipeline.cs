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
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.UI.GraphicalDecorators
{
    public class GraphicalPipeline : INotifyCollectionChanged, IEnumerable<IGraphical>
    {
        private readonly PipeLine pipeLine;
        private readonly IList<GraphicalElement> nodes = new List<GraphicalElement>();
        private readonly IList<GraphicalSinkPad> sinkPads = new List<GraphicalSinkPad>();
        private readonly IList<GraphicalSrcPad> srcPads = new List<GraphicalSrcPad>();
        private readonly IList<GraphicalEdge> edges = new List<GraphicalEdge>();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Remove(GraphicalElement graphicalElement)
        {
            pipeLine.Remove(graphicalElement.Element);

            var sinks = this.sinkPads.Where(sink => sink.Parent == graphicalElement).ToList();
            var srcs = this.srcPads.Where(src => src.Parent == graphicalElement).ToList();


            foreach (var sink in sinks)
            {
                Remove(sink);
                
            }

            foreach (var src in srcs)
            {
                Remove(src);
            }

            var index = GetIndex(graphicalElement);
            this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, graphicalElement, index));
            this.nodes.Remove(graphicalElement);
        }

        private void Remove(GraphicalEdge edge)
        {
            int index = GetIndex(edge);
            this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge, index));
            this.edges.Remove(edge);
        }

        private void Remove(GraphicalSinkPad sink)
        {
            var edges = this.edges.Where(edge => edge.Sink == sink).ToList();

            foreach (var edge in edges)
            {
                 Remove(edge);
            }

            int index = GetIndex(sink);
            this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, sink, index));
            this.sinkPads.Remove(sink);
        }

        private void Remove(GraphicalSrcPad src)
        {

            var edges = this.edges.Where(edge => edge.Src == src).ToList();

            foreach (var edge in edges)
            {
                Remove(edge);
            }

            int index = GetIndex(src);
            this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, src, index));
            this.srcPads.Remove(src);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public GraphicalPipeline(PipeLine pipeLine)
        {
            this.pipeLine = pipeLine;
        }

        public IPipeElement CreateNodeFromTemplate(IPipeElement template, Point position)
        {
            var node = this.pipeLine.CreateNodeFromTemplate(template);
            this.AddNode(node, position);
            return node;
        }

        

        private void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged.Invoke(this, args);
            }
        }

        public void AddNode(IPipeElement node, Point position)
        {
            this.pipeLine.Add(node);
            var graphicalNode = new GraphicalElement(node, position, this);
            this.nodes.Add(graphicalNode);
            this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, graphicalNode, GetIndex(graphicalNode)));
            int padNr = 0;
            foreach (var sink in node.GetSinkPads())
            {
                var gsink = new GraphicalSinkPad(sink, graphicalNode, padNr++);
                this.sinkPads.Add(gsink);
                this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, gsink, GetIndex(gsink)));
            }
            padNr = 0;
            foreach (var src in node.GetSrcPads())
            {
                var gsrc = new GraphicalSrcPad(src, graphicalNode, padNr++);
                this.srcPads.Add(gsrc);
                this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, gsrc, GetIndex(gsrc)));
            }
        }

        public void Connect<TValue>(PipeSrcPad<TValue> From, PipeSinkPad<TValue> To)
        {
            this.pipeLine.Connect<TValue>(From, To);
            var from = this.srcPads.Single(src => src.Element == From);
            var to = this.sinkPads.Single(sink => sink.Element == To);
            AddEdge(from, to);
        }

        private void AddEdge(GraphicalSrcPad src, GraphicalSinkPad sink)
        {
            var edge = new GraphicalEdge(src, sink);
            this.edges.Add(edge);
            this.FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge, GetIndex(edge)));
        }

        public Task Start()
        {
            return this.pipeLine.Start();
        }

        public Task Stop()
        {
            return this.pipeLine.Stop();
        }

        internal void TryConnect(GraphicalSrcPad src, GraphicalSinkPad sink)
        {
            if(this.pipeLine.TryConnect(src.Element, sink.Element))
            {
                GraphicalEdge? sinkEdge = this.edges.SingleOrDefault((e) => e.Sink == sink);
                GraphicalEdge? srcEdge = this.edges.SingleOrDefault((e) => e.Src == src);

                if(sinkEdge != null)
                {
                    this.Remove(sinkEdge);
                }
                if (srcEdge != null)
                {
                    this.Remove(srcEdge);
                }

                this.AddEdge(src, sink);
            }
        }

        private int GetIndex(GraphicalElement node)
        {
            return this.nodes.IndexOf(node);
        }

        private int GetIndex(GraphicalSinkPad sink)
        {
            return this.sinkPads.IndexOf(sink) + this.nodes.Count;
        }

        private int GetIndex(GraphicalSrcPad src)
        {
            return this.srcPads.IndexOf(src) + this.nodes.Count + this.sinkPads.Count;
        }

        private int GetIndex(GraphicalEdge edge)
        {
            return this.edges.IndexOf(edge) + this.nodes.Count + this.sinkPads.Count + this.srcPads.Count;
        }

        public IEnumerator<IGraphical> GetEnumerator()
        {
            var elements = this.nodes.Cast<IGraphical>();
            var sinks = this.sinkPads.Cast<IGraphical>();
            var srcs = this.srcPads.Cast<IGraphical>();
            var edges = this.edges.Cast<IGraphical>();
            return elements.Concat(sinks).Concat(srcs).Concat(edges).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal PipeLineDefinition GetDefinition()
        {
            return pipeLine.GetDefinition();
        }
    }
}
