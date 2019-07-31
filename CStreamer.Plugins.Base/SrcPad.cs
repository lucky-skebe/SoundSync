// -----------------------------------------------------------------------
// <copyright file="SrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
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
    /// Base class for all SrcPads.
    ///
    /// Data always flows from <see cref="SrcPad{TValue}"/> to <see cref="SinkPad{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The typt of value this pad can push throu the pipeline.</typeparam>
    public class SrcPad<TValue> : ISrcPad<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SrcPad{TValue}"/> class.
        /// </summary>
        /// <param name="parent">the element this pad is connected to.</param>
        /// <param name="name">the name of the pad.</param>
        /// <param name="mandatory">A value indicating whether the Pad needs to be linked for the element to be functional.</param>
        public SrcPad(IElement parent, string name, bool mandatory)
        {
            this.Parent = parent;
            this.Name = name;
            this.Mandatory = mandatory;
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
        public ISinkPad<TValue>? Peer { get; private set; }

        IPad? IPad.Peer => this.Peer;

        ISinkPad? ISrcPad.Peer => this.Peer;

        ISinkPad<TValue>? ISrcPad<TValue>.Peer => this.Peer;

        /// <inheritdoc/>
        public bool Mandatory { get; }

        /// <inheritdoc/>
        public bool IsLinked()
        {
            return this.Peer != null;
        }

        /// <summary>
        /// Pushed data along the pipeline.
        /// </summary>
        /// <param name="value">the value to push into towards the connected <see cref="SinkPad{TValue}"/>.</param>
        public void Push(TValue value)
        {
            if (this.Peer != null)
            {
                this.Peer?.Push(value);
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
        public bool Equals(ISrcPad other)
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
        public Option<ISinkPad<TValue>, string> Link(ISinkPad<TValue> peer)
        {
            if (peer == this.Peer)
            {
                return Option.Some<ISinkPad<TValue>, string>(peer);
            }

            this.Peer = peer;

            this.Peer.Link(this);

            return Option.Some<ISinkPad<TValue>, string>(peer);
        }

        /// <inheritdoc/>
        public Option<ISinkPad, string> Link(ISinkPad peer)
        {
            if (peer == this.Peer)
            {
                return Option.Some<ISinkPad, string>(peer);
            }

            if (peer is ISinkPad<TValue> truePeer)
            {
                this.Peer = truePeer;
                truePeer.Link(this);
                return Option.Some<ISinkPad, string>(peer);
            }
            else
            {
                return Option.None<ISinkPad, string>("Could not link Pads be casue the types didn't match");
            }
        }

        /// <inheritdoc/>
        public Option<IPad, string> Link(IPad peer)
        {
            if (peer == this.Peer)
            {
                return Option.Some<IPad, string>(peer);
            }

            if (peer is ISinkPad<TValue> truePeer)
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
