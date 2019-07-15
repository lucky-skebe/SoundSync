// -----------------------------------------------------------------------
// <copyright file="ElementAddedEventArgs.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.Events
{
    /// <summary>
    /// Provides data for the <see cref="PipeLine.ElementAdded" /> event.
    /// </summary>
    public class ElementAddedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAddedEventArgs"/> class.
        /// </summary>
        /// <param name="element">The added element.</param>
        public ElementAddedEventArgs(IPipeElement element)
        {
            this.Element = element;
        }

        /// <summary>
        /// Gets wich element was added.
        /// </summary>
        /// <value>
        /// the added element.
        /// </value>
        public IPipeElement Element { get; }
    }
}
