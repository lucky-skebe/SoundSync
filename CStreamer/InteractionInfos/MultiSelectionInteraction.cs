// -----------------------------------------------------------------------
// <copyright file="MultiSelectionInteraction.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.InteractionInfos
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// An interaction that contains multiple selectable items.
    /// Usually shown in a listbox wich a checkbox for each item.
    /// </summary>
    /// <typeparam name="TSelect">type of the selectable.</typeparam>
    /// <typeparam name="TValue">type of the selecables inner element.</typeparam>
    public abstract class MultiSelectionInteraction<TSelect, TValue> : IInteraction
        where TSelect : ISelectable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectionInteraction{TSelect, TValue}"/> class.
        /// </summary>
        /// <param name="name">the name of the interaction.</param>
        protected MultiSelectionInteraction(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets a list of <see cref="ISelectable{T}"/> items.
        /// </summary>
        /// <value>
        /// A list of <see cref="ISelectable{T}"/> items.
        /// </value>
        public ObservableCollection<TSelect> Options { get; } = new ObservableCollection<TSelect>();

        /// <inheritdoc/>
        public string Name { get; }
    }
}
