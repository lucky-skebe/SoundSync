// -----------------------------------------------------------------------
// <copyright file="ILink.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    /// <summary>
    /// Describes the link between two pads.
    /// </summary>
    public interface ILink
    {
        /// <summary>
        /// Gets the srs side pad of the link.
        /// </summary>
        /// <value>
        /// The srs side pad of the link.
        /// </value>
        ISrcPad Src { get; }

        /// <summary>
        /// Gets the sink side pad of the link.
        /// </summary>
        /// <value>
        /// The sink side pad of the link.
        /// </value>
        ISinkPad Sink { get; }

        /// <summary>
        /// Unlinks the elements connected by this link.
        /// </summary>
        void Unlink();
    }

    public interface ILink<TValue> : ILink
    {
        /// <summary>
        /// Gets the srs side pad of the link.
        /// </summary>
        /// <value>
        /// The srs side pad of the link.
        /// </value>
        new ISrcPad<TValue> Src { get; }

        /// <summary>
        /// Gets the sink side pad of the link.
        /// </summary>
        /// <value>
        /// The sink side pad of the link.
        /// </value>
        new ISinkPad<TValue> Sink { get; }

        void Push(TValue value);
    }
}
