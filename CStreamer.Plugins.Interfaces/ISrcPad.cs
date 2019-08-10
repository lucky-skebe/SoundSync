// -----------------------------------------------------------------------
// <copyright file="ISrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    using System;
    using Optional;

    /// <summary>
    /// Describes the minimum requirements for a SrcPad.
    /// </summary>
    public interface ISrcPad : IPad, IEquatable<ISrcPad>
    {
        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        new ISinkPad? Peer { get; }

        /// <summary>
        /// Gets the output type this pad can produce.
        /// </summary>
        /// <value>
        /// The output type this pad can produce.
        /// </value>
        PadOutput Output { get; }

        /// <summary>
        /// Links this Pad to the provided peer pad.
        /// </summary>
        /// <param name="peer">the peerpad to link to.</param>
        /// <returns>An <see cref="Option{T, TException}"/> contianing either the new peer or an error message.</returns>
        Option<ISinkPad, string> Link(ISinkPad peer);
    }

    /// <summary>
    /// Describes the minimum requirements for a SrcPad that produces a certein type.
    /// </summary>
    /// <typeparam name="TValue" >The type this SrcPad produces.</typeparam>
    public interface ISrcPad<TValue> : ISrcPad
    {
        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        new ISinkPad<TValue>? Peer { get; }

        /// <summary>
        /// Links this Pad to the provided peer pad.
        /// </summary>
        /// <param name="peer">the peerpad to link to.</param>
        /// <returns>An <see cref="Option{T, TException}"/> contianing either the new peer or an error message.</returns>
        Option<ISinkPad<TValue>, string> Link(ISinkPad<TValue> peer);
    }
}
