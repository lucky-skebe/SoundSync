// -----------------------------------------------------------------------
// <copyright file="SrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;

    /// <summary>
    /// Base class for all SrcPads.
    ///
    /// Data always flows from <see cref="SrcPad{TValue}"/> to <see cref="PipeSinkPad{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The typt of value this pad can push throu the pipeline.</typeparam>
    public class SrcPad<TValue> : ISrcPad<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SrcPad{TValue}"/> class.
        /// </summary>
        /// <param name="parent">the element this pad is connected to.</param>
        /// <param name="name">the name of the pad.</param>
        public SrcPad(ISrcElement parent, string name)
        {
            this.Parent = parent;
            this.Name = name;
        }

        /// <inheritdoc/>
        public ISrcElement Parent
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
        public ISinkPad<TValue>? Peer => this.Edge?.Sink;

        IPad? IPad.Peer => this.Peer;

        ISinkPad? ISrcPad.Peer => this.Peer;

        ISinkPad<TValue>? ISrcPad<TValue>.Peer => this.Peer;

        protected ILink<TValue>? Edge { get; set; }

        ISrcElement ISrcPad.Parent => this.Parent;

        IElement IPad.Parent => this.Parent;

        /// <inheritdoc/>
        public bool IsLinked()
        {
            return this.Edge != null;
        }

        /// <summary>
        /// Pushed data along the pipeline.
        /// </summary>
        /// <param name="value">the value to push into towards the connected <see cref="PipeSinkPad{TValue}"/>.</param>
        public void Push(TValue value)
        {
            if (this.Edge != null)
            {
                this.Edge.Push(value);
            }
        }

        /// <inheritdoc/>
        public void Unlink()
        {
            this.Edge?.Unlink();
            this.Edge = null;
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
    }
}
