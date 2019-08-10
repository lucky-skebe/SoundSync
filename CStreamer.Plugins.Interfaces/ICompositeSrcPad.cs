// -----------------------------------------------------------------------
// <copyright file="ICompositeSrcPad.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// An Interface for all Pads that can be composed of multiple child pads.
    /// </summary>
    /// <seealso cref="CStreamer.Plugins.Interfaces.ISrcPad" />
    public interface ICompositeSrcPad : ISrcPad
    {
        /// <summary>
        /// Gets the child pads of this pad.
        /// </summary>
        /// <value>
        /// The child pads of this pad.
        /// </value>
        public List<ISrcPad> ChildPads { get; }
    }
}
