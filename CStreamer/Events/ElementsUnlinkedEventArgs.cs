﻿// -----------------------------------------------------------------------
// <copyright file="ElementsUnlinkedEventArgs.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Events
{
    using CStreamer.Plugins.Interfaces;

    /// <summary>
    /// Provides data for the <see cref="PipeLine.ElementsUnlinked" /> event.
    /// </summary>
    public class ElementsUnlinkedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementsUnlinkedEventArgs"/> class.
        /// </summary>
        /// <param name="src">The src pad that was unlinked.</param>
        /// <param name="sink">The sink pad that was unlinked.</param>
        public ElementsUnlinkedEventArgs(ISrcPad src, ISinkPad sink)
        {
            this.Src = src;
            this.Sink = sink;
        }

        /// <summary>
        /// Gets the unlinked src pad.
        /// </summary>
        /// <value>
        /// The unlinked src pad.
        /// </value>
        public ISrcPad Src { get; }

        /// <summary>
        /// Gets the unlinked sink pad.
        /// </summary>
        /// <value>
        /// The unlinked sink pad.
        /// </value>
        public ISinkPad Sink { get; }
    }
}
