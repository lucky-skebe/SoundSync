// -----------------------------------------------------------------------
// <copyright file="IPropertyBinding.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces
{
    using System;
    using Optional;

    /// <summary>
    /// Describes the methods all PropertyBinding should implement.
    /// Ususally <see cref="IPropertyBinding{TValue}"/> shoule be used when writing custom bindings.
    /// </summary>
    public interface IPropertyBinding
    {
        /// <summary>
        /// Occurs when the Value of the bound property changes.
        /// </summary>
        public event EventHandler<BindingValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Gets the name of the Property.
        /// </summary>
        /// <value>
        /// The name of the Property.
        /// </value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Tries to set the value of a specified property.
        /// </summary>
        /// <param name="propvalue">the name and value of the property to set.</param>
        /// <returns>True if the property was set. False otherwise.</returns>
        Option<object?, string> TrySetValue(PropertyValue propvalue);

        /// <summary>
        /// Gets the name and value of a property.
        /// </summary>
        /// <returns>The name and value of a property.</returns>
        PropertyValue GetValue();
    }

    /// <summary>
    /// Describes the methods all PropertyBinding should implement.
    /// </summary>
    /// <typeparam name="TValue">The Type of the bound property.</typeparam>
    public interface IPropertyBinding<TValue> : IPropertyBinding
    {
        /// <summary>
        /// Occurs when the Value of the bound property changes.
        /// </summary>
        public new event EventHandler<BindingValueChangedEventArgs<TValue>> ValueChanged;

        /// <summary>
        /// Gets or sets the current Value of the bound Property.
        /// </summary>
        /// <value>
        /// The current Value of the bound Property.
        /// </value>
        public TValue Value { get; set; }
    }
}
