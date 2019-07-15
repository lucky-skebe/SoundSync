// -----------------------------------------------------------------------
// <copyright file="ISelectable.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.InteractionInfos
{
    /// <summary>
    /// Describes an Entry in a <see cref="MultiSelectionInteraction{TSelect, TValue}"/>.
    /// It Contains a Value and information if that value is Selected.
    /// </summary>
    /// <typeparam name="T">The of the underlying Value.</typeparam>
    public interface ISelectable<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Option is selected.
        /// </summary>
        /// <value>
        /// A value indicating whether the Option is selected.
        /// </value>
        bool Selected { get; set; }

        /// <summary>
        /// Gets the underlying Value.
        /// </summary>
        /// <value>
        /// The underlying Value.
        /// </value>
        T Value { get; }
    }
}
