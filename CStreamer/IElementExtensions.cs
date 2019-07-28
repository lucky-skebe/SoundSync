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

    public static class IElementExtensions
    {
        public static IEnumerable<ISrcPad> GetSrcPads(this IElement element)
        {
            return element.GetPads().OfType<ISrcPad>();
        }

        public static IEnumerable<ISrcPad<TValue>> GetSrcPads<TValue>(this IElement element)
        {
            return element.GetSrcPads().OfType<ISrcPad<TValue>>();
        }

        public static IEnumerable<ISinkPad> GetSinkPads(this IElement element)
        {
            return element.GetPads().OfType<ISinkPad>();
        }

        public static IEnumerable<ISinkPad<TValue>> GetSinkPads<TValue>(this IElement element)
        {
            return element.GetSinkPads().OfType<ISinkPad<TValue>>();
        }

        public static Option<ISrcPad> GetSrcPad(this IElement element, string name)
        {
            return element.GetSrcPads().SingleOrNone(p => p.Name == name);
        }

        public static Option<ISrcPad<TValue>> GetSrcPad<TValue>(this IElement element, string name)
        {
            return element.GetSrcPads<TValue>().SingleOrNone(p => p.Name == name);
        }

        public static Option<ISinkPad> GetSinkPad(this IElement element, string name)
        {
            return element.GetSinkPads().SingleOrNone(p => p.Name == name);
        }

        public static Option<ISinkPad<TValue>> GetSinkPad<TValue>(this IElement element, string name)
        {
            return element.GetSinkPads<TValue>().SingleOrNone(p => p.Name == name);
        }

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

        public static IEnumerable<IElement> GetPrevElements(this IElement element)
        {
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
            return element.GetSinkPads().Select(p => p.Peer).Where(peer => peer != null).Select(p => p.Parent);
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.
        }

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
