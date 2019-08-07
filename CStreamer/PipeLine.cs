// -----------------------------------------------------------------------
// <copyright file="PipeLine.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using CStreamer.Base;
    using CStreamer.Events;
    using CStreamer.PipeLineDefinitions;
    using CStreamer.Plugins.Interfaces;
    using CStreamer.Plugins.Interfaces.Messages;
    using Optional;

    /// <summary>
    /// A complete pipeline consisting of linked elements that transform data from one form to another.
    /// </summary>
    public class PipeLine : IBin
    {
        private readonly IList<IElement> elements = new List<IElement>();

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
        public static IElement CreateNodeFromTemplate(IElement template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            IElement node = (IElement)template.GetType().GetConstructor(new Type[] { typeof(string) }).Invoke(new object?[] { null });
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
                pipeline.Elements.Add(new ElementDefinition(element.GetElementName(), element.Name, element.GetPropertyValues().ToList()));

                foreach (var srcPad in element.GetSrcPads())
                {
                    if (srcPad.Peer != null)
                    {
                        pipeline.Links.Add(new LinkDefinition(srcPad.Parent.Name, srcPad.Name, srcPad.Peer.Parent.Name, srcPad.Peer.Name));
                    }
                }
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
            Dictionary<string, IElement> elementCache = new Dictionary<string, IElement>();

            foreach (var element in definition.Elements)
            {
                IElement? pipeElement = PipeElementFactory.Make(element.TypeFactory, element.Name);
                if (pipeElement != null)
                {
                    this.Add(pipeElement);

                    foreach (var propvalue in element.Properties)
                    {
                        pipeElement
                            .SetPropertyValue(propvalue)
                            .MatchNone((error) => errors.Add(error));
                    }

                    elementCache.Add(element.Name, pipeElement);
                }
            }

            foreach (var link in definition.Links)
            {
                IElement? fromElement = elementCache[link.FromElement];
                IElement? toElement = elementCache[link.ToElement];

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

                Option<ISrcPad> srcPad = fromElement.GetSrcPad(link.FromPad);
                Option<ISinkPad> sinkPad = toElement.GetSinkPad(link.ToPad);

                srcPad.Match(
                    (srcPad) =>
                    {
                        sinkPad.Match(
                            (sinkPad) =>
                            {
                                if (!this.TryConnect(srcPad, sinkPad))
                                {
                                    errors.Add($"Could not Link to {link.ToElement}:{link.ToPad} because the types don't match");
                                }
                            },
                            () =>
                            {
                                errors.Add($"Could not Link to {link.ToElement}:{link.ToPad} because the Pad doesn't exist");
                            });
                    },
                    () =>
                    {
                        errors.Add($"Could not Link from {link.FromElement}:{link.FromPad} because the Pad doesn't exist");
                    });
            }

            return errors;
        }

        /// <summary>
        /// Add an element to the pipeline to be handled.
        /// The pipeline handles the <see cref="State"/> of all it's elements and serielizes elements it owns when it gets serialized.
        /// </summary>
        /// <param name="element">The element to add to the pipeline.</param>
        public void Add(IElement element)
        {
            this.elements.Add(element);
            element.Parent = this;
            this.OnElementAdded(element);
        }

        /// <summary>
        /// Removes an element from the pipeline.
        /// This unlinks the element from all other elements.
        /// Afterwars this element won't be handled by the pipleline.
        /// </summary>
        /// <param name="element">The element to remove from the pipeline.</param>
        public void Remove(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            foreach (IPad pad in element.GetPads())
            {
                pad.Unlink();
            }

            this.elements.Remove(element);
            element.Parent = null;
            this.OnElementRemoved(element);
        }

        /// <summary>
        /// Connect two element pads, letting them send data from the src to the sink.
        /// </summary>
        /// <typeparam name="TValue">The type of data that can be sent.</typeparam>
        /// <param name="src">The src ot the data.</param>
        /// <param name="sink">The destination of the data.</param>
        public void Connect<TValue>(ISrcPad<TValue> src, ISinkPad<TValue> sink)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            src.Link(sink);

            this.OnElementsLinked(src, sink);
        }

        /// <summary>
        /// Tries to Connect two pads if you don't have strongly typed pads.
        /// </summary>
        /// <param name="src">The src pad sending data.</param>
        /// <param name="sink">The sink pad receiving the data.</param>
        /// <returns>True if the elements were linked. False otherwise.</returns>
        /// <exception cref="ArgumentNullException">If either parameters were null.</exception>
        public bool TryConnect(ISrcPad src, ISinkPad sink)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (sink == null)
            {
                throw new ArgumentNullException(nameof(sink));
            }

            this.Unlink(sink);
            this.Unlink(src);

            var result = src.Link(sink);

            result.MatchNone(s => this.ReceiveMessage(new ErrorMessage(s)));

            return result.HasValue;
        }

        /// <summary>
        /// Moves the pipeline and all it's elements to the <see cref="State.Playing"/> state.
        /// </summary>
        /// <returns>A task that represents the state change operation.</returns>
        public Task<Option<State, IEnumerable<string>>> Start()
        {
            return this.GoToState(State.Playing);
        }

        /// <summary>
        /// Moves the pipeline and all it's elements to the <see cref="State.Stopped"/> state.
        /// </summary>
        /// <returns>A task that represents the state change operation.</returns>
        public Task<Option<State, IEnumerable<string>>> Stop()
        {
            return this.GoToState(State.Stopped);
        }

        /// <summary>
        /// Tries to change the pipelines state to the state given
        ///
        /// Going throu all the states inbetween in order and sets the state
        /// of all contained elements.
        /// </summary>
        /// <param name="state">The desired State.</param>
        /// <returns>The new State after the change or a List of Errors that need to be fixed.</returns>
        public async Task<Option<State, IEnumerable<string>>> GoToState(State state)
        {
            var transitions = StateManager.GetTransitions(this.currentState, state);

            // Sync All elements to current state without resetting if they are already in a future state
            transitions.Insert(0, this.currentState);

            var orderedElementOptions = this.GetOrderedElements();

            bool shouldReverse = IsReverseOrder(this.currentState, state);

            return await orderedElementOptions.Match<Task<Option<State, IEnumerable<string>>>>(
                async orderedElements =>
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

                    return Option.Some<State, IEnumerable<string>>(state);
                },
                (errors) => Task.FromResult(Option.None<State, IEnumerable<string>>(errors))).ConfigureAwait(true);
        }

        /// <inheritdoc/>
        public void ReceiveMessage(Message message)
        {
            if (message is PadsLinkedMessage padsLinked)
            {
                this.OnElementsLinked(padsLinked.SrcPad, padsLinked.SinkPad);
            }
            else if (message is PadsUnlinkedMessage padsUnlinked)
            {
                this.OnElementsUnlinked(padsUnlinked.SrcPad, padsUnlinked.SinkPad);
            }
            else if (message is ErrorMessage error)
            {
                // this.OnElementsLinked(padsLinked.SrcPad, padsLinked.SinkPad);
            }
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

        private void Clear()
        {
            while (this.elements.Count > 0)
            {
                this.Remove(this.elements[0]);
                foreach (var pad in this.elements[0].GetPads())
                {
                    pad.Unlink();
                }
            }
        }

        private void OnElementAdded(IElement element)
        {
            this.ElementAdded?.Invoke(this, new ElementAddedEventArgs(element));
        }

        private void OnElementRemoved(IElement element)
        {
            this.ElementRemoved?.Invoke(this, new ElementRemovedEventArgs(element));
        }

        private void OnElementsLinked(ISrcPad src, ISinkPad sink)
        {
            this.ElementsLinked?.Invoke(this, new ElementsLinkedEventArgs(src, sink));
        }

        private void OnElementsUnlinked(ISrcPad src, ISinkPad sink)
        {
            this.ElementsUnlinked?.Invoke(this, new ElementsUnlinkedEventArgs(src, sink));
        }

        private Option<List<IElement>, List<string>> GetOrderedElements()
        {
            List<IElement> orderedList = new List<IElement>();
            List<string> errors = new List<string>();

            // TODO Refactor to use an autogenerated node name instead of the nodes hash
            Dictionary<IElement, IList<IElement>> prevNodeList = new Dictionary<IElement, IList<IElement>>();
            Dictionary<IElement, IList<IElement>> nextNodeList = new Dictionary<IElement, IList<IElement>>();
            IList<IElement> startNodes = new List<IElement>();

            foreach (var element in this.elements)
            {
                var state = element.CheckLinks();
                if (!state)
                {
                    errors.Add($"Element {element.Name} has unlinked Pads");
                    continue;
                }

                var prevNodes = element.GetPrevElements();

                bool nodeAdded = false;

                foreach (var prev in prevNodes)
                {
                    if (!prevNodeList.ContainsKey(prev))
                    {
                        prevNodeList.Add(prev, new List<IElement>());
                    }

                    if (!nextNodeList.ContainsKey(element))
                    {
                        nextNodeList.Add(element, new List<IElement>());
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
                errors.Add("Pipeline contains Cycles");
            }

            if (errors.Count > 0)
            {
                return Option.None<List<IElement>, List<string>>(errors);
            }

            return Option.Some<List<IElement>, List<string>>(orderedList);
        }

        private void Unlink(ISrcPad srcPad)
        {
            var peer = srcPad.Peer;
            if (peer != null)
            {
                srcPad.Unlink();

                this.OnElementsUnlinked(srcPad, peer);
            }
        }

        private void Unlink(ISinkPad sinkPad)
        {
            var peer = sinkPad.Peer;
            if (peer != null)
            {
                sinkPad.Unlink();
                this.OnElementsUnlinked(peer, sinkPad);
            }
        }
    }
}
