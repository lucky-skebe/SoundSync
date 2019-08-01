// -----------------------------------------------------------------------
// <copyright file="PropertyValue.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer
{
    /// <summary>
    /// The value of an element property. used for serialization/deserialization.
    /// </summary>
    public class PropertyValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValue"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        public PropertyValue(string propertyName, object? value)
        {
            this.PropertyName = propertyName;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <value>
        /// The value of the property.
        /// </value>
        public object? Value { get; }
    }
}
