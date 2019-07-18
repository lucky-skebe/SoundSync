// -----------------------------------------------------------------------
// <copyright file="ValuePropertyInteraction.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.InteractionInfos
{
    using System;

    /// <summary>
    /// Describes the Interaction with an Elements Property.
    /// </summary>
    /// <typeparam name="TValue">The Type of the underlying Property.</typeparam>
    public abstract class ValuePropertyInteraction<TValue> : IInteraction
    {
        private readonly Func<TValue> getValue;
        private readonly Action<TValue> setValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuePropertyInteraction{TValue}"/> class.
        /// </summary>
        /// <param name="name">The name of the Interaction.</param>
        /// <param name="getValue">The method get get the propertys value.</param>
        /// <param name="setValue">The method get set the propertys value.</param>
        protected ValuePropertyInteraction(string name, Func<TValue> getValue, Action<TValue> setValue)
        {
            this.Name = name;
            this.getValue = getValue;
            this.setValue = setValue;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the Value of the Elements Property.
        /// </summary>
        /// <value>
        /// The Value of the Elements Property.
        /// </value>
        public TValue Value
        {
            get => this.getValue();
            set => this.setValue(value);
        }
    }
}
