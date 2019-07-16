﻿// -----------------------------------------------------------------------
// <copyright file="ISinkPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;

    /// <summary>
    /// Describes the minimum requirements for a SinkPad.
    /// most of the time <see cref="PipeSinkPad{TValue}"/> should be used.
    /// </summary>
    public interface ISinkPad : IPad, IEquatable<ISinkPad>
    {
        /// <summary>
        /// Gets the element this pad is a part of.
        /// </summary>
        /// <value>
        /// The element this pad is a part of.
        /// </value>
        new ISinkElement Parent
        {
            get;
        }

        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        new ISrcPad? Peer { get; }
    }

    public interface ISinkPad<TValue> : ISinkPad
    {
        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        new ISrcPad<TValue>? Peer { get; }
    }
}
