// -----------------------------------------------------------------------
// <copyright file="IPropertyBinding.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using SharPipes.Pipes.Base.PipeLineDefinitions;

    /// <summary>
    /// Describes the methods all PropertyBinding should implement.
    /// Ususally <see cref="PropertyBinding{TValue}"/> shoule be used when writing custom bindings.
    /// </summary>
    public interface IPropertyBinding
    {
        /// <summary>
        /// Tries to set the value of a specified property.
        /// </summary>
        /// <param name="propvalue">the name and value of the property to set.</param>
        /// <returns>True if the property was set. False otherwise.</returns>
        bool TrySetValue(PropertyValue propvalue);

        /// <summary>
        /// Gets the name and value of a property.
        /// </summary>
        /// <returns>The name and value of a property.</returns>
        PropertyValue GetValue();
    }
}
