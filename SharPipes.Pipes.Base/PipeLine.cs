﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public class PipeLine
    {
        private readonly IList<IPipeElement> nodes = new List<IPipeElement>();
        private readonly IList<IPipeEdge> edges = new List<IPipeEdge>();

        public void AddNode(IPipeElement node)
        {
            nodes.Add(node);
        }

        private (GraphState, List<IPipeElement>?) GetOrderedElements()
        {
            List<IPipeElement>? orderedList = new List<IPipeElement>();
            // TODO Refactor to use an autogenerated node name instead of the nodes hash
            Dictionary<IPipeElement, IList<IPipeElement>> prevNodeList = new Dictionary<IPipeElement, IList<IPipeElement>>();
            Dictionary<IPipeElement, IList<IPipeElement>> nextNodeList = new Dictionary<IPipeElement, IList<IPipeElement>>();
            IList<IPipeElement> startNodes = new List<IPipeElement>();

            foreach (var node in nodes)
            {
                var state = node.Check();
                if (state != GraphState.OK)
                {
                    return (state, null);
                }

                var prevNodes = node.GetPrevNodes();

                bool nodeAdded = false;

                foreach (var prev in prevNodes)
                {
                    if (!prevNodeList.ContainsKey(prev))
                    {
                        prevNodeList.Add(prev, new List<IPipeElement>());
                    }
                    if (!nextNodeList.ContainsKey(node))
                    {
                        nextNodeList.Add(node, new List<IPipeElement>());
                    }

                    prevNodeList[prev].Add(node);
                    nextNodeList[node].Add(prev);
                    nodeAdded = true;
                }

                if(!nodeAdded){
                    
                    startNodes.Add(node);
                }
            }

            while(startNodes.Count > 0)
            {
                var node = startNodes[0];
                startNodes.RemoveAt(0);

                orderedList.Add(node);

                if (prevNodeList.ContainsKey(node))
                {
                    while ( prevNodeList[node].Count > 0)
                    {
                        var prevNode = prevNodeList[node][0];
                        prevNodeList[node].Remove(prevNode);
                        nextNodeList[prevNode].Remove(node);

                        if(nextNodeList[prevNode].Count == 0)
                        {
                            nextNodeList.Remove(prevNode);
                            startNodes.Add(prevNode);
                        }
                    }
                    prevNodeList.Remove(node);
                }
            }

            if(prevNodeList.Count > 0 || nextNodeList.Count > 0 || startNodes.Count > 0)
            {
                return (GraphState.CYCLE, null);
            }

            return (GraphState.OK, orderedList);
        }

        public void Connect<TValue>(PipeSrcPad<TValue> From, PipeSinkPad<TValue> To)
        {
            PipeEdge<TValue> e = new PipeEdge<TValue>(From, To);
            From.Edge = e;
            To.Edge = e;
            this.edges.Add(e);
        }

        public async Task Start()
        {
            var (state, orderedElements) = GetOrderedElements();
            if(state == GraphState.OK && orderedElements!= null)
            {
                orderedElements.Reverse();

                foreach (var elem in orderedElements)
                {
                    await elem.Start();
                }
            }
        }

        public async Task Stop()
        {
            var (state, orderedElements) = GetOrderedElements();
            if (state == GraphState.OK && orderedElements != null)
            {
                foreach (var elem in orderedElements)
                {
                    await elem.Stop();
                }
            }
        }
    }
}
