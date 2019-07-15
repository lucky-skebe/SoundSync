// -----------------------------------------------------------------------
// <copyright file="PipeLine.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using SharPipes.Pipes.Base.Events;
    using SharPipes.Pipes.Base.PipeLineDefinitions;

    /// <summary>
    /// A complete pipeline consisting of linked elements that transform data from one form to another.
    /// </summary>
    public class PipeLine
    {
        private readonly IList<IPipeElement> elements = new List<IPipeElement>();

        private readonly IList<IPipeLink> links = new List<IPipeLink>();

        private State currentState = State.Stopped;

        /// <summary>
        /// Occurs when an element gets added to the pipeline.
        /// </summary>
        public event EventHandler<ElementAddedEventArgs> ElementAdded;

        /// <summary>
        /// Occurs when an element gets removed to the pipeline.
        /// </summary>
        public event EventHandler<ElementRemovedEventArgs> ElementRemoved;

        /// <summary>
        /// Occurs when two element get linked.
        /// </summary>
        public event EventHandler<ElementsLinkedEventArgs> ElementsLinked;

        /// <summary>
        /// Occurs when two element get unlinked.
        /// </summary>
        public event EventHandler<ElementsUnlinkedEventArgs> ElementsUnlinked;

        /// <summary>
        /// Creates a new element from an existing template.
        /// Only the type gets copied.
        /// Properties need to be initialized manually.
        /// </summary>
        /// <param name="template">The template element.</param>
        /// <returns>A new element of the same type as the template.</returns>
        public static IPipeElement CreateNodeFromTemplate(IPipeElement template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            IPipeElement node = (IPipeElement)template.GetType().GetConstructor(new Type[] { typeof(string?) }).Invoke(new object?[] { null });
            return node;
        }

        /// <summary>
        /// Gets a description of the current pipeline including all off the pipelines elements, element properties and links.
        /// Used for serializing the pipeline.
        /// </summary>
        /// <returns>The pipelines desciption.</returns>
        public PipeLineDefinition GetDefinition()
        {
            var pipeline = new PipeLineDefinition();

            foreach (var element in this.elements)
            {
                pipeline.Elements.Add(new ElementDefinition(PipeElementFactory.GetName(element.GetType()), element.Name, element.GetPropertyValues().ToList()));
            }

            foreach (var link in this.links)
            {
                pipeline.Links.Add(new LinkDefinition(link.Src.Parent.Name, link.Src.Name, link.Sink.Parent.Name, link.Sink.Name));
            }

            return pipeline;
        }

        /// <summary>
        /// Clears the pipeline and recreates itself using the given description.
        /// USed for deserializing the pipeline.
        /// </summary>
        /// <param name="definition">the pipelinee definition.</param>
        /// <returns>A list of errors that occured turing the builing of the pipeline.</returns>
        public IList<string> FromDefinition(PipeLineDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            IList<string> errors = new List<string>();

            this.Clear();
            Dictionary<string, IPipeElement> elementCache = new Dictionary<string, IPipeElement>();

            foreach (var element in definition.Elements)
            {
                IPipeElement? pipeElement = PipeElementFactory.Make(element.TypeFactory, element.Name);
                if (pipeElement != null)
                {
                    this.Add(pipeElement);

                    foreach (var propvalue in element.Properties)
                    {
                        if (!pipeElement.SetPropertyValue(propvalue))
                        {
                            errors.Add($"Property {propvalue} could not be set.");
                        }
                    }

                    elementCache.Add(element.Name, pipeElement);
                }
            }

            foreach (var link in definition.Links)
            {
                IPipeElement? fromElement = elementCache[link.FromElement];
                IPipeElement? toElement = elementCache[link.ToElement];

                if (fromElement == null)
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

                if (!this.TryConnect(srcPad, sinkPad))
                {
                    errors.Add($"Could not Link to {link.ToElement}:{link.ToPad} because the types don't match");
                    continue;
                }
            }

            return errors;
        }

        /// <summary>
        /// Add an element to the pipeline to be handled.
        /// The pipeline handles the <see cref="State"/> of all it's elements and serielizes elements it owns when it gets serialized.
        /// </summary>
        /// <param name="element">The element to add to the pipeline.</param>
        public void Add(IPipeElement element)
        {
            this.elements.Add(element);
            this.OnElementAdded(element);
        }

        /// <summary>
        /// Removes an element from the pipeline.
        /// This unlinks the element from all other elements.
        /// Afterwars this element won't be handled by the pipleline.
        /// </summary>
        /// <param name="element">The element to remove from the pipeline.</param>
        public void Remove(IPipeElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            foreach (IPipeSinkPad sink in element.GetSinkPads())
            {
                this.Unlink(sink);
            }

            foreach (var src in element.GetSrcPads())
            {
                this.Unlink(src);
            }

            this.elements.Remove(element);
            this.OnElementRemoved(element);
        }

        /// <summary>
        /// Connect two element pads, letting them send data from the src to the sink.
        /// </summary>
        /// <typeparam name="TValue">The type of data that can be sent.</typeparam>
        /// <param name="src">The src ot the data.</param>
        /// <param name="sink">The destination of the data.</param>
        public void Connect<TValue>(PipeSrcPad<TValue> src, PipeSinkPad<TValue> sink)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            PipeEdge<TValue> e = new PipeEdge<TValue>(src, sink);
            src.Edge = e;
            sink.Edge = e;
            this.links.Add(e);
            this.OnElementsLinked(src, sink);
        }

        /// <summary>
        /// Tries to Connect two pads if you don't have strongly typed pads.
        /// </summary>
        /// <param name="src">The src pad sending data.</param>
        /// <param name="sink">The sink pad receiving the data.</param>
        /// <returns>True if the elements were linked. False otherwise.</returns>
        /// <exception cref="ArgumentNullException">If either parameters were null.</exception>
        public bool TryConnect(IPipeSrcPad src, IPipeSinkPad sink)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            var srcType = src.GetType();
            var sinkType = sink.GetType();

            var srcBaseType = IsInstanceOfGenericType(typeof(PipeSrcPad<>), srcType);
            var sinkBaseType = IsInstanceOfGenericType(typeof(PipeSinkPad<>), sinkType);

            if (srcBaseType == null)
            {
                return false;
            }

            if (sinkBaseType == null)
            {
                return false;
            }

            if (!srcBaseType.GenericTypeArguments.SequenceEqual(sinkBaseType.GenericTypeArguments))
            {
                return false;
            }

            MethodInfo? genericConnect = typeof(PipeLine).GetMethod(nameof(this.Connect));
            if (genericConnect == null)
            {
                return false;
            }

            var specificConnect = genericConnect.MakeGenericMethod(srcBaseType.GenericTypeArguments);

            this.Unlink(sink);
            this.Unlink(src);
            specificConnect.Invoke(this, new object[] { src, sink });

            return true;
        }

        /// <summary>
        /// Moves the pipeline and all it's elements to the <see cref="State.Playing"/> state.
        /// </summary>
        /// <returns>A task that represents the state change operation.</returns>
        public Task Start()
        {
            return this.GoToState(State.Playing);
        }

        /// <summary>
        /// Moves the pipeline and all it's elements to the <see cref="State.Stopped"/> state.
        /// </summary>
        /// <returns>A task that represents the state change operation.</returns>
        public Task Stop()
        {
            return this.GoToState(State.Stopped);
        }

        private static bool IsReverseOrder(State from, State to)
        {
            return (from, to) switch
            {
                (State.Stopped, _) => true,
                (State.Ready, State.Playing) => true,
                _ => false
            };
        }

        private static Type? IsInstanceOfGenericType(Type genericType, Type instanceType)
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

        private void Clear()
        {
            while (this.elements.Count > 0)
            {
                this.Remove(this.elements[0]);
            }

            this.links.Clear();
        }

        private void OnElementAdded(IPipeElement element)
        {
            this.ElementAdded?.Invoke(this, new ElementAddedEventArgs(element));
        }

        private void OnElementRemoved(IPipeElement element)
        {
            this.ElementRemoved?.Invoke(this, new ElementRemovedEventArgs(element));
        }

        private void OnElementsLinked(IPipeSrcPad src, IPipeSinkPad sink)
        {
            this.ElementsLinked?.Invoke(this, new ElementsLinkedEventArgs(src, sink));
        }

        private void OnElementsUnlinked(IPipeSrcPad src, IPipeSinkPad sink)
        {
            this.ElementsUnlinked?.Invoke(this, new ElementsUnlinkedEventArgs(src, sink));
        }

        private (GraphState, List<IPipeElement>?) GetOrderedElements()
        {
            List<IPipeElement>? orderedList = new List<IPipeElement>();

            // TODO Refactor to use an autogenerated node name instead of the nodes hash
            Dictionary<IPipeElement, IList<IPipeElement>> prevNodeList = new Dictionary<IPipeElement, IList<IPipeElement>>();
            Dictionary<IPipeElement, IList<IPipeElement>> nextNodeList = new Dictionary<IPipeElement, IList<IPipeElement>>();
            IList<IPipeElement> startNodes = new List<IPipeElement>();

            foreach (var element in this.elements)
            {
                var state = element.Check();
                if (state != GraphState.OK)
                {
                    return (state, null);
                }

                var prevNodes = element.GetPrevNodes();

                bool nodeAdded = false;

                foreach (var prev in prevNodes)
                {
                    if (!prevNodeList.ContainsKey(prev))
                    {
                        prevNodeList.Add(prev, new List<IPipeElement>());
                    }

                    if (!nextNodeList.ContainsKey(element))
                    {
                        nextNodeList.Add(element, new List<IPipeElement>());
                    }

                    prevNodeList[prev].Add(element);
                    nextNodeList[element].Add(prev);
                    nodeAdded = true;
                }

                if (!nodeAdded)
                {
                    startNodes.Add(element);
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

        private async Task GoToState(State state)
        {
            var transitions = StateManager.GetTransitions(this.currentState, state);

            // Sync All elements to current state without resetting if they are already in a future state
            transitions.Insert(0, this.currentState);

            var (elemstate, orderedElements) = this.GetOrderedElements();

            bool shouldReverse = IsReverseOrder(this.currentState, state);

            if (elemstate != GraphState.CYCLE && orderedElements != null)
            {
                if (shouldReverse)
                {
                    orderedElements.Reverse();
                }

                int step = 0;
                foreach (State transition in transitions)
                {
                    foreach (var elem in orderedElements)
                    {
                        var stateIndex = transitions.IndexOf(elem.CurrentState);
                        if (stateIndex < step)
                        {
                            await elem.GoToState(transition).ConfigureAwait(true);
                        }
                    }

                    step++;
                    this.currentState = transition;
                }
            }
        }
    }
}
