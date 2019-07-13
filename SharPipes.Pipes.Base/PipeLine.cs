using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SharPipes.Pipes.Base.Events;
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.Pipes.Base
{
    public class PipeLine
    {
        
        public PipeLineDefinition GetDefinition()
        {
            var pipeline = new PipeLineDefinition();

            foreach( var element in this.elements)
            {
                pipeline.Elements.Add(new ElementDefinition(PipeElementFactory.GetName(element.GetType()), element.Name, element.GetPropertyValues().ToList()));
            }

            foreach (var link in this.links)
            {
                pipeline.Links.Add(new LinkDefinition(link.From.Parent.Name, link.From.Name, link.To.Parent.Name, link.To.Name ));
            }

            return pipeline;
        }

        private void Clear()
        {
            while(this.elements.Count > 0)
            {
                this.Remove(this.elements[0]);
            }

            this.links.Clear();
        }

        public IList<string> FromDefinition(PipeLineDefinition definition)
        {
            IList<string> errors = new List<string>();

            this.Clear();
            Dictionary<string, IPipeElement> elementCache = new Dictionary<string, IPipeElement>();

            foreach(var element in definition.Elements)
            {
                IPipeElement? pipeElement = PipeElementFactory.Make(element.TypeFactory, element.Name);
                if (pipeElement != null)
                {
                    this.Add(pipeElement);
                    
                    foreach(var propvalue in element.Properties)
                    {
                        if(!pipeElement.SetPropertyValue(propvalue))
                        {
                            errors.Add($"Property {propvalue} could not be set.");
                        }
                    }

                    elementCache.Add(element.Name, pipeElement);
                }
            }

            foreach(var link in definition.Links)
            {
                IPipeElement? fromElement = elementCache[link.FromElement];
                IPipeElement? toElement = elementCache[link.ToElement];

                if(fromElement == null)
                {
                    errors.Add($"Could not Link from {link.FromElement}:{link.FromPad} because the Element doesn't exist");
                    continue;
                }
                if (fromElement == null)
                {
                    errors.Add($"Could not Link to {link.ToElement}:{link.ToPad} because the Element doesn't exist");
                    continue;
                }

                IPipeSrcPad? srcPad = fromElement.GetSrcPad(link.FromPad);
                IPipeSinkPad? sinkPad = toElement.GetSinkPad(link.ToPad);

                if (srcPad == null)
                {
                    errors.Add($"Could not Link from {link.FromElement}:{link.FromPad} because the Pad doesn't exist");
                    continue;
                }
                if (sinkPad == null)
                {
                    errors.Add($"Could not Link to {link.ToElement}:{link.ToPad} because the Pad doesn't exist");
                    continue;
                }

                if(!this.TryConnect(srcPad, sinkPad))
                {
                    errors.Add($"Could not Link to {link.ToElement}:{link.ToPad} because the types don't match");
                    continue;
                }

            }

            return errors;
        }
        public event EventHandler<ElementAddedEventArgs> ElementAdded;
        public event EventHandler<ElementRemovedEventArgs> ElementRemoved;
        public event EventHandler<ElementsLinkedEventArgs> ElementsLinked;
        public event EventHandler<ElementsUnlinkedEventArgs> ElementsUnlinked;

        private void OnElementAdded(IPipeElement element)
        {
            ElementAdded?.Invoke(this, new ElementAddedEventArgs(element));
        }

        private void OnElementRemoved(IPipeElement element)
        {
            ElementRemoved?.Invoke(this, new ElementRemovedEventArgs(element));
        }

        private void OnElementsLinked(IPipeSrcPad src, IPipeSinkPad sink)
        {
            ElementsLinked?.Invoke(this, new ElementsLinkedEventArgs(src, sink));
        }

        private void OnElementsUnlinked(IPipeSrcPad src, IPipeSinkPad sink)
        {
            ElementsUnlinked?.Invoke(this, new ElementsUnlinkedEventArgs(src, sink));
        }

        private readonly IList<IPipeElement> elements = new List<IPipeElement>();
        private readonly IList<IPipeEdge> links = new List<IPipeEdge>();

        public void Add(IPipeElement element)
        {
            elements.Add(element);
            this.OnElementAdded(element);
        }

        public IPipeElement CreateNodeFromTemplate(IPipeElement template)
        {
            IPipeElement node = (IPipeElement)template.GetType().GetConstructor(new Type[] { typeof(String?) }).Invoke(new object[] { null });
            return node;
        }

        private (GraphState, List<IPipeElement>?) GetOrderedElements()
        {
            List<IPipeElement>? orderedList = new List<IPipeElement>();
            // TODO Refactor to use an autogenerated node name instead of the nodes hash
            Dictionary<IPipeElement, IList<IPipeElement>> prevNodeList = new Dictionary<IPipeElement, IList<IPipeElement>>();
            Dictionary<IPipeElement, IList<IPipeElement>> nextNodeList = new Dictionary<IPipeElement, IList<IPipeElement>>();
            IList<IPipeElement> startNodes = new List<IPipeElement>();

            foreach (var node in elements)
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

                if (!nodeAdded) {

                    startNodes.Add(node);
                }
            }

            while (startNodes.Count > 0)
            {
                var node = startNodes[0];
                startNodes.RemoveAt(0);

                orderedList.Add(node);

                if (prevNodeList.ContainsKey(node))
                {
                    while (prevNodeList[node].Count > 0)
                    {
                        var prevNode = prevNodeList[node][0];
                        prevNodeList[node].Remove(prevNode);
                        nextNodeList[prevNode].Remove(node);

                        if (nextNodeList[prevNode].Count == 0)
                        {
                            nextNodeList.Remove(prevNode);
                            startNodes.Add(prevNode);
                        }
                    }
                    prevNodeList.Remove(node);
                }
            }

            if (prevNodeList.Count > 0 || nextNodeList.Count > 0 || startNodes.Count > 0)
            {
                return (GraphState.CYCLE, null);
            }

            return (GraphState.OK, orderedList);
        }

        private void Unlink(IPipeSrcPad srcPad)
        {
            var peer = srcPad.Peer;
            if (peer != null)
            {
                
                srcPad.Unlink();
                
                this.OnElementsUnlinked(srcPad, peer);
            }
        }

        private void Unlink(IPipeSinkPad sinkPad)
        {
            var peer = sinkPad.Peer;
            if (peer != null)
            {
                sinkPad.Unlink();
                this.OnElementsUnlinked(peer, sinkPad);
            }
        }

        public void Remove(IPipeElement element)
        {
            foreach( var sink in element.GetSinkPads())
            {
                Unlink(sink);
            }

            foreach (var src in element.GetSrcPads())
            {
                Unlink(src);
            }

            this.elements.Remove(element);
            this.OnElementRemoved(element);
        }

        public void Connect<TValue>(PipeSrcPad<TValue> From, PipeSinkPad<TValue> To)
        {
            PipeEdge<TValue> e = new PipeEdge<TValue>(From, To);
            From.Edge = e;
            To.Edge = e;
            this.links.Add(e);
            this.OnElementsLinked(From, To);
        }

        public State CurrentState = State.Stopped;

        private async Task GoToState(State state)
        {
            var transitions = StateManager.GetTransitions(this.CurrentState, state);

            //Sync All elements to current state without resetting if they are already in a future state
            transitions.Insert(0, this.CurrentState);

            var (elemstate, orderedElements) = GetOrderedElements();

            bool shouldReverse = IsReverseOrder(this.CurrentState, state);

            if (elemstate != GraphState.CYCLE && orderedElements != null)
            {
                if (shouldReverse)
                {
                    orderedElements.Reverse();
                }
                int step = 0;
                foreach(State transition in transitions)
                {
                    foreach (var elem in orderedElements)
                    {
                        var stateIndex = transitions.IndexOf(elem.CurrentState);
                        if (stateIndex < step)
                        {
                            await elem.GoToState(transition);
                        }
                    }
                    step++;
                    this.CurrentState = transition;
                }
            }

            
        }

        private bool IsReverseOrder(State from, State to)
        {
            return (from, to) switch
            {
                (State.Stopped, _) => true,
                (State.Ready, State.Playing) => true,
                _ => false
            };
        }

        public Task Start()
        {
            return GoToState(State.Playing);
        }

        static Type? IsInstanceOfGenericType(Type genericType, Type instanceType)
        {
            while (instanceType != null)
            {
                if (instanceType.IsGenericType &&
                    instanceType.GetGenericTypeDefinition() == genericType)
                {
                    return instanceType;
                }
                instanceType = instanceType.BaseType;
            }
            return null;
        }

        public bool TryConnect(IPipeSrcPad src, IPipeSinkPad sink)
        {
            var srcType = src.GetType();
            var sinkType = sink.GetType();

            var srcBaseType = IsInstanceOfGenericType(typeof(PipeSrcPad<>), srcType);
            var sinkBaseType = IsInstanceOfGenericType(typeof(PipeSinkPad<>), sinkType);

            if (srcBaseType == null)
            {
                return false;
            } 
            if(sinkBaseType == null)
            {
                return false;
            }
            if(!srcBaseType.GenericTypeArguments.SequenceEqual(sinkBaseType.GenericTypeArguments))
            {
                return false;
            }

            MethodInfo? genericConnect = typeof(PipeLine).GetMethod(nameof(Connect));
            if (genericConnect == null)
            {
                return false;
            }

            var specificConnect = genericConnect.MakeGenericMethod(srcBaseType.GenericTypeArguments);
                        
            Unlink(sink);
            Unlink(src);
            specificConnect.Invoke(this, new object[] { src, sink });

            return true;
        }




        public Task Stop()
        {
            return this.GoToState(State.Stopped);
        }
    }
}
