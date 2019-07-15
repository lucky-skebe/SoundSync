// -----------------------------------------------------------------------
// <copyright file="Selectable.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.InteractionInfos
{
    using System;

    /// <summary>
    /// Describes an Entry in a <see cref="MultiSelectionInteraction{TSelect, TValue}"/>.
    /// It Contains a Value and information if that value is Selected.
    /// </summary>
    /// <typeparam name="T">The of the underlying Value.</typeparam>
    public class Selectable<T> : ISelectable<T>
    {
        private readonly Action<Selectable<T>, bool>? setSelectedCallback;

        private bool selected = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Selectable{T}"/> class.
        /// </summary>
        /// <param name="value">The underlying Value.</param>
        /// <param name="setSelectedCallback">A callback that is called each time the IsSelected Property Changes.</param>
        public Selectable(T value, Action<Selectable<T>, bool>? setSelectedCallback = null)
        {
            this.Value = value;
            this.setSelectedCallback = setSelectedCallback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Selectable{T}"/> class.
        /// </summary>
        /// <param name="value">The underlying Value.</param>
        /// <param name="selected">the initial selection state.</param>
        /// <param name="setSelectedCallback">A callback that is called each time the IsSelected Property Changes.</param>
        public Selectable(T value, bool selected, Action<Selectable<T>, bool>? setSelectedCallback = null)
        {
            this.selected = selected;
            this.Value = value;
            this.setSelectedCallback = setSelectedCallback;
        }

        /// <inheritdoc/>
        public bool Selected
        {
            get => this.selected;
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.setSelectedCallback?.Invoke(this, value);
                }
            }
        }

        /// <inheritdoc/>
        public T Value { get; }
    }
}
