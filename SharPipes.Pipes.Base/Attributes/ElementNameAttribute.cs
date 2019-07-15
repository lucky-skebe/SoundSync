// -----------------------------------------------------------------------
// <copyright file="ElementNameAttribute.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.Attributes
{
    using System;

    /// <summary>
    /// USed to set the name an element can be retrieved by useing <see cref="PipeElementFactory.Make(string, string)"/>.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class ElementNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The name to register under.</param>
        public ElementNameAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name to register under.
        /// </summary>
        /// <value>
        /// The name to register under.
        /// </value>
        public string Name { get; }
    }
}
