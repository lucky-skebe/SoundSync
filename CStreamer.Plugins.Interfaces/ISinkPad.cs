// -----------------------------------------------------------------------
// <copyright file="ISinkPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    using System;
    using Optional;

    /// <summary>
    /// Describes the minimum requirements for a SinkPad.
    /// </summary>
    public interface ISinkPad : IPad, IEquatable<ISinkPad>
    {
        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        new ISrcPad? Peer { get; }

        PadFilter Filter { get; }

        /// <summary>
        /// Links this Pad to the provided peer pad.
        /// </summary>
        /// <param name="peer">the peerpad to link to.</param>
        /// <returns>An <see cref="Option{T, TException}"/> contianing either the new peer or an error message.</returns>
        Option<ISrcPad, string> Link(ISrcPad peer);
    }

    /// <summary>
    /// Describes the minimum requirements for a SinkPad that accepts a certein type.
    /// </summary>
    /// <typeparam name="TValue" >The type this SinkPad accepts.</typeparam>
    public interface ISinkPad<TValue> : ISinkPad
    {
        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        new ISrcPad<TValue>? Peer { get; }

        /// <summary>
        /// Consumes a Value and sends it to it's parent element.
        /// </summary>
        /// <param name="value">The value to send to it's parent.</param>
        void Push(TValue value);

        /// <summary>
        /// Links this Pad to the provided peer pad.
        /// </summary>
        /// <param name="peer">the peerpad to link to.</param>
        /// <returns>An <see cref="Option{T, TException}"/> contianing either the new peer or an error message.</returns>
        Option<ISrcPad<TValue>, string> Link(ISrcPad<TValue> peer);
    }
}
