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
    /// most of the time <see cref="PipeSrcPad{TValue}"/> should be used.
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

        Option<ISinkPad, string> Link(ISinkPad peer);
    }

    public interface ISrcPad<TValue> : ISrcPad
    {
        new ISinkPad<TValue>? Peer { get; }

        Option<ISinkPad<TValue>, string> Link(ISinkPad<TValue> peer);
    }
}
