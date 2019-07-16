// -----------------------------------------------------------------------
// <copyright file="ElementsLinkedEventArgs.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.Events
{
    /// <summary>
    /// Provides data for the <see cref="PipeLine.ElementsLinked" /> event.
    /// </summary>
    public class ElementsLinkedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementsLinkedEventArgs"/> class.
        /// </summary>
        /// <param name="src">The src pad that was linked.</param>
        /// <param name="sink">The sink pad that was linked.</param>
        public ElementsLinkedEventArgs(ISrcPad src, ISinkPad sink)
        {
            this.Src = src;
            this.Sink = sink;
        }

        /// <summary>
        /// Gets the linked src pad.
        /// </summary>
        /// <value>
        /// The linked src pad.
        /// </value>
        public ISrcPad Src { get; }

        /// <summary>
        /// Gets the linked sink pad.
        /// </summary>
        /// <value>
        /// The linked sink pad.
        /// </value>
        public ISinkPad Sink { get; }
    }
}
