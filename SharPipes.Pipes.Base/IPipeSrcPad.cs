// -----------------------------------------------------------------------
// <copyright file="IPipeSrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;

    /// <summary>
    /// Describes the minimum requirements for a SrcPad.
    /// most of the time <see cref="PipeSrcPad{TValue}"/> should be used.
    /// </summary>
    public interface IPipeSrcPad : IEquatable<IPipeSrcPad>
    {
        /// <summary>
        /// Gets the element this pad is a part of.
        /// </summary>
        /// <value>
        /// The element this pad is a part of.
        /// </value>
        public IPipeSrc Parent
        {
            get;
        }

        /// <summary>
        /// Gets the Name of the pad. Usually in the format of [ElementName]-[PadName]
        /// Examlple:
        /// Multiply-00000000-0000-0000-0000-000000000000-Sink.
        /// </summary>
        /// <value>
        /// The Name of the pad.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the pad on the other side of the link of null if the pad is not linked.
        /// </summary>
        /// <value>
        /// The pad on the other side of the link of null if the pad is not linked.
        /// </value>
        IPipeSinkPad? Peer { get; }

        /// <summary>
        /// Returns the linking status of the pad.
        /// </summary>
        /// <returns>true if the pad is linked, false otherwise.</returns>
        bool IsLinked();

        /// <summary>
        /// Unliks the pad from it's peer pad.
        /// </summary>
        public void Unlink();
    }
}
