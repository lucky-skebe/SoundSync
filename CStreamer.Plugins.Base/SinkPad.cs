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
        /// <param name="mandatory"></param>
        public SinkPad(IElement parent, string name, Action<TValue> elementCallback, bool mandatory)
        {
            this.Name = name;
            this.ElementCallback = elementCallback;
            this.Parent = parent;
            this.Mandatory = mandatory;
        }

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

        IElement IPad.Parent => this.Parent;

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

            return this.Parent.Equals(other.Parent) && this.Name.Equals(other.Name, StringComparison.Ordinal);
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

            this.Peer = peer;

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
                this.Peer = truePeer;
                return Option.Some<ISrcPad, string>(peer);
            }
            else
            {
                return Option.None<ISrcPad, string>("Could not link Pads be casue the types didn't match");
            }
        }

        /// <inheritdoc/>
        public Option<IPad, string> Link(IPad peer)
        {
            if (peer == this.Peer)
            {
                return Option.Some<IPad, string>(peer);
            }

            if (peer is ISrcPad<TValue> truePeer)
            {
                this.Peer = truePeer;
                return Option.Some<IPad, string>(peer);
            }
            else
            {
                return Option.None<IPad, string>("Could not link Pads be casue the types didn't match");
            }
        }
    }
}
