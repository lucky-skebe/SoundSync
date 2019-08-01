// -----------------------------------------------------------------------
// <copyright file="ElementDefinition.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.PipeLineDefinitions
{
    using System.Collections.Generic;

    /// <summary>
    /// This class is the description of one element used for serializing and deserializing elements.
    /// </summary>
    public class ElementDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementDefinition"/> class.
        /// </summary>
        /// <param name="typeFactory">The name used to recreate the element this object describes.</param>
        /// <param name="name">The name of the element.</param>
        /// <param name="properties">All the properties of the described element.</param>
        public ElementDefinition(string typeFactory, string name, IList<PropertyValue> properties)
        {
            this.TypeFactory = typeFactory;
            this.Name = name;
            this.Properties = properties;
        }

        /// <summary>
        /// Gets the name used to recreate the element this object describes.
        /// </summary>
        /// <value>
        /// The name used to recreate the element this object describes.
        /// A new element of the same Type can be created using the <see cref="PipeElementFactory.Make(string, string)"/> method.
        /// </value>
        public string TypeFactory { get; }

        /// <summary>
        /// Gets the name of the element this object describes.
        /// </summary>
        /// <value>
        /// The name of the element this object describes.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets all properties of the element this object describes.
        /// </summary>
        /// <value>
        /// All properties of the element this object describes.
        /// </value>
        public IList<PropertyValue> Properties { get; }
    }
}
