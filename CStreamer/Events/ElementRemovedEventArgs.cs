// -----------------------------------------------------------------------
// <copyright file="ElementRemovedEventArgs.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using CStreamer.Plugins.Interfaces;

namespace CStreamer.Events
{
    /// <summary>
    /// Provides data for the <see cref="PipeLine.ElementRemoved" /> event.
    /// </summary>
    public class ElementRemovedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementRemovedEventArgs"/> class.
        /// </summary>
        /// <param name="element">The removed element.</param>
        public ElementRemovedEventArgs(IElement element)
        {
            this.Element = element;
        }

        /// <summary>
        /// Gets wich element was removed.
        /// </summary>
        /// <value>
        /// the removed element.
        /// </value>
        public IElement Element { get; }
    }
}
