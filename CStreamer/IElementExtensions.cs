// -----------------------------------------------------------------------
// <copyright file="IElementExtensions.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CStreamer.Plugins.Interfaces;
    using Optional;
    using Optional.Collections;

    /// <summary>
    /// Extends <see cref="IElement"/> to give it some additional funcionality.
    /// </summary>
    public static class IElementExtensions
    {
        /// <summary>
        /// Returns all the SrcPads of the given Element.
        /// </summary>
        /// <param name="element">The element to get the SrcPads from.</param>
        /// <returns>All the SrcPads.</returns>
        /// <exception cref="ArgumentNullException">If element is null.</exception>
        public static IEnumerable<ISrcPad> GetSrcPads(this IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element.GetPads().OfType<ISrcPad>();
        }

        /// <summary>
        /// Returns all the SrcPads of the given Element that have a given Type.
        /// </summary>
        /// <param name="element">The element to get the SinkPads from.</param>
        /// <typeparam name="TValue">The Type of SinkPads to return.</typeparam>
        /// <returns>All the SrcPads.</returns>
        /// <exception cref="ArgumentNullException">If element is null.</exception>
        public static IEnumerable<ISrcPad<TValue>> GetSrcPads<TValue>(this IElement element)
        {
            return element.GetSrcPads().OfType<ISrcPad<TValue>>();
        }

        /// <summary>
        /// Returns all the SinkPads of the given Element.
        /// </summary>
        /// <param name="element">The element to get the SinkPads from.</param>
        /// <returns>All the SinkPads.</returns>
        /// <exception cref="ArgumentNullException">If element is null.</exception>
        public static IEnumerable<ISinkPad> GetSinkPads(this IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element.GetPads().OfType<ISinkPad>();
        }

        /// <summary>
        /// Returns all the SinkPads of the given Element that have a given Type.
        /// </summary>
        /// <param name="element">The element to get the SinkPads from.</param>
        /// <typeparam name="TValue">The Type of SinkPads to return.</typeparam>
        /// <returns>All the SrcPads.</returns>
        /// <exception cref="ArgumentNullException">If element is null.</exception>
        public static IEnumerable<ISinkPad<TValue>> GetSinkPads<TValue>(this IElement element)
        {
            return element.GetSinkPads().OfType<ISinkPad<TValue>>();
        }

        /// <summary>
        /// Gets a SrcPad based on it's name.
        /// </summary>
        /// <param name="element">the element to get the pad from.</param>
        /// <param name="name">the name of the SrcPad.</param>
        /// <returns>An <see cref="Option"/> containing the Pad.</returns>
        public static Option<ISrcPad> GetSrcPad(this IElement element, string name)
        {
            return element.GetSrcPads().SingleOrNone(p => p.Name == name);
        }

        /// <summary>
        /// Gets a SrcPad based on it's name if if's of a given Type.
        /// </summary>
        /// <typeparam name="TValue">The Type the Pad should be.</typeparam>
        /// <param name="element">The element to get the pad from.</param>
        /// <param name="name">The name of the SrcPad.</param>
        /// <returns>An <see cref="Option"/> containing the Pad.</returns>
        public static Option<ISrcPad<TValue>> GetSrcPad<TValue>(this IElement element, string name)
        {
            return element.GetSrcPads<TValue>().SingleOrNone(p => p.Name == name);
        }

        /// <summary>
        /// Gets a SinkPad based on it's name.
        /// </summary>
        /// <param name="element">the element to get the pad from.</param>
        /// <param name="name">the name of the SinkPad.</param>
        /// <returns>An <see cref="Option"/> containing the Pad.</returns>
        public static Option<ISinkPad> GetSinkPad(this IElement element, string name)
        {
            return element.GetSinkPads().SingleOrNone(p => p.Name == name);
        }

        /// <summary>
        /// Gets a SinkPad based on it's name if if's of a given Type.
        /// </summary>
        /// <typeparam name="TValue">Thh Type the Pad should be.</typeparam>
        /// <param name="element">The element to get the pad from.</param>
        /// <param name="name">The name of the SinkPad.</param>
        /// <returns>An <see cref="Option"/> containing the Pad.</returns>
        public static Option<ISinkPad<TValue>> GetSinkPad<TValue>(this IElement element, string name)
        {
            return element.GetSinkPads<TValue>().SingleOrNone(p => p.Name == name);
        }

        /// <summary>
        /// Set an elements property based on a <see cref="PropertyValue"/>.
        /// </summary>
        /// <param name="element">The element to set a property.</param>
        /// <param name="propertyValue">The PropertyValue containing the propertyinformation.</param>
        /// <returns>An <see cref="Option"/> containing either the new Value or an error message.</returns>
        public static Option<object?, string> SetPropertyValue(this IElement element, PropertyValue propertyValue)
        {
            if (propertyValue == null)
            {
                throw new ArgumentNullException(nameof(propertyValue));
            }

            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element
                .GetPropertyBindings()
                .FirstOrNone(binding => binding.Name == propertyValue.PropertyName)
                .WithException($"Could not find Property {propertyValue.PropertyName}")
                .FlatMap(binding => binding.TrySetValue(propertyValue));
        }

        /// <summary>
        /// Tries to retriev a properties value given a propertyname.
        /// </summary>
        /// <param name="element">The element to retrieve the data from.</param>
        /// <param name="name">The name of the Property.</param>
        /// <returns>An <see cref="Option"/> contianing either the value or an errormessage.</returns>
        public static Option<object?, string> GetPropertyValue(this IElement element, string name)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element
                .GetPropertyBindings()
                .FirstOrNone(binding => binding.Name == name)
                .WithException($"Could not find Property {name}")
                .Map<object?>(binding => binding.GetValue().Value);
        }

        /// <summary>
        /// Retrieves all PRopertyvalues that should be persisted.
        ///
        /// USed for Serialization.
        /// </summary>
        /// <param name="element">The element to get the properties of.</param>
        /// <returns>A list of all the property Names and Values.</returns>
        public static IEnumerable<PropertyValue> GetPropertyValues(this IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element
                .GetPropertyBindings()
                .Select(binding => binding.GetValue());
        }

        /// <summary>
        /// Returns all elements that feed data directly into the element.
        /// </summary>
        /// <param name="element">the element to check.</param>
        /// <returns>A List of direct parent elements.</returns>
        /// <exception cref="ArgumentNullException"> if element is null.</exception>
        public static IEnumerable<IElement> GetPrevElements(this IElement element)
        {
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
            return element.GetSinkPads().Select(p => p.Peer).Where(peer => peer != null).Select(p => p.Parent);
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.
        }

        /// <summary>
        /// Checks if any mandatory Pads are unlinked.
        /// </summary>
        /// <param name="element">The Element to check.</param>
        /// <returns>True if all mandatory PAds are linked otherwise False.</returns>
        public static bool CheckLinks(this IElement element)
        {
            if (element == null)
            {
                return true;
            }

            return !element.GetPads().Where(p => p.Mandatory).Any(p => p.Peer == null);
        }
    }
}
