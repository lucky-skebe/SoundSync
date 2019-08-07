// -----------------------------------------------------------------------
// <copyright file="SinkPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using System;
    using CStreamer.Plugins.Interfaces;
    using CStreamer.Plugins.Interfaces.Messages;
    using Optional;

    /// <summary>
    /// Base class for all SinkPads.
    ///
    /// Data always flows from <see cref="SrcPad{TValue}"/> to <see cref="SinkPad{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The typt of value this pad can accept.</typeparam>
    public class SinkPad<TValue> : ISinkPad<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SinkPad{TValue}"/> class.
        /// </summary>
        /// <param name="parent">the element this pad is connected to.</param>
        /// <param name="name">the name of the pad.</param>
        /// <param name="elementCallback">the callback inside the element to push data to.</param>
        /// <param name="mandatory">A value indicating whether the Pad needs to be linked for the element to be functional.</param>
        public SinkPad(IElement parent, string name, Action<TValue> elementCallback, bool mandatory, PadFilter? filter = null)
        {
            this.Name = name;
            this.ElementCallback = elementCallback;
            this.Parent = parent;
            this.Mandatory = mandatory;

            this.Filter = filter ?? new PadFilter { Content = PadContent.Any(), Format = PadFormat.Any() };

        }

        /// <inheritdoc/>
        public IElement Parent
        {
            get;
            protected set;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        public ISrcPad<TValue>? Peer { get; private set; }

        ISrcPad? ISinkPad.Peer => this.Peer;

        ISrcPad<TValue>? ISinkPad<TValue>.Peer => this.Peer;

        IPad? IPad.Peer => this.Peer;

        /// <inheritdoc/>
        public bool Mandatory { get; }

        /// <summary>
        /// Gets or sets the element callback.
        /// </summary>
        /// <value>
        /// The element callback.
        /// this callback is called each time a value is pushed from the connected edge.
        /// </value>
        protected Action<TValue> ElementCallback
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public string Caps => $"{typeof(TValue).Name} ({Filter.ToString()})";

        public PadFilter Filter { get; }

        /// <inheritdoc/>
        public bool IsLinked()
        {
            return this.Peer != null;
        }

        /// <summary>
        /// Pushed data along the pipeline.
        /// </summary>
        /// <param name="value">the value to push into the element.</param>
        public void Push(TValue value)
        {
            if (this.Parent.CurrentState == State.Playing)
            {
                this.ElementCallback(value);
            }
        }

        /// <inheritdoc/>
        public void Unlink()
        {
            var peer = this.Peer;
            this.Peer = null;
            peer?.Unlink();
        }

        /// <inheritdoc/>
        public bool Equals(ISinkPad other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Parent.Equals(other.Parent) && this.Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (this.Parent, this.Name).GetHashCode();
        }

        /// <inheritdoc/>
        public Option<ISrcPad<TValue>, string> Link(ISrcPad<TValue> peer)
        {
            if (peer == this.Peer)
            {
                return Option.Some<ISrcPad<TValue>, string>(peer);
            }

            if (!this.Filter.CanAccept(peer.Output))
            {
                return Option.None<ISrcPad<TValue>, string>("Format not Supported");
            }

            this.Peer = peer;
            this.Parent.SendMessage(new PadsLinkedMessage(peer, this));

            this.Peer.Link(this);

            return Option.Some<ISrcPad<TValue>, string>(peer);
        }

        /// <inheritdoc/>
        public Option<ISrcPad, string> Link(ISrcPad peer)
        {
            if (peer == this.Peer)
            {
                return Option.Some<ISrcPad, string>(peer);
            }

            if (peer is ISrcPad<TValue> truePeer)
            {
                return this.Link(truePeer).Map<ISrcPad>(p => p);
            }
            else if (peer is ICompositeSrcPad composite)
            {
                foreach (var childPad in composite.ChildPads)
                {
                    var result = this.Link(childPad);
                    if (result.HasValue)
                    {
                        this.Parent.SendMessage(new PadsLinkedMessage(composite, this));
                        return result;
                    }
                }

                return Option.None<ISrcPad, string>("Could not link Pads be casue the there was no matching ChildPad");
            }
            else
            {
                return Option.None<ISrcPad, string>("Could not link Pads be casue the types didn't match");
            }
        }

        /// <inheritdoc/>
        public Option<IPad, string> Link(IPad peer)
        {
            if (peer is ISrcPad srcPeer)
            {
                return this.Link(srcPeer).Map<IPad>(p => p);
            }
            else
            {
                return Option.None<IPad, string>("Could not link Pads be casue the types didn't match");
            }
        }
    }
}
