// -----------------------------------------------------------------------
// <copyright file="IElementExtensions.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using CStreamer.Plugins.Attributes;
    using CStreamer.Plugins.Interfaces;

    public static class IElementExtensions
    {
        /// <summary>
        /// Gets the name for a given Elements type.
        ///
        /// These names can either be registered using the <see cref="ElementNameAttribute"/> or will be generated using the Classname.
        /// Classnames ending in Src, Sink, or Element will get these parts removed.
        /// </summary>
        /// <param name="element">the element to resolve the name of.</param>
        /// <returns>The factoryType name of the given type.</returns>
        /// <exception cref="ArgumentNullException">if type is null.</exception>
        public static string GetElementName(this IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var type = element.GetType();

            return GetElementName(type);
        }

        /// <summary>
        /// Gets the name for a given Type.
        ///
        /// These names can either be registered using the <see cref="ElementNameAttribute"/> or will be generated using the Classname.
        /// Classnames ending in Src, Sink, or Element will get these parts removed.
        /// </summary>
        /// <param name="type">the type to resolve the name of.</param>
        /// <returns>The factoryType name of the given type.</returns>
        /// <exception cref="ArgumentNullException">if type is null.</exception>
        public static string GetElementName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            ElementNameAttribute? attribute;
            if ((attribute = type.GetCustomAttribute<ElementNameAttribute>()) != null)
            {
                return attribute.Name;
            }
            else
            {
                var typeName = type.Name;

                if (TrimEnd(typeName, "element", out string trimmed))
                {
                    return trimmed;
                }
                else if (TrimEnd(typeName, "src", out trimmed))
                {
                    return trimmed;
                }
                else if (TrimEnd(typeName, "sink", out trimmed))
                {
                    return trimmed;
                }

                return type.Name;
            }
        }

        private static bool TrimEnd(string from, string end, out string trimmed)
        {
            trimmed = from;
            if (from.EndsWith(end, StringComparison.OrdinalIgnoreCase))
            {
                var index = from.LastIndexOf(end, StringComparison.OrdinalIgnoreCase);
                trimmed = from.Substring(0, index);
                return true;
            }

            return false;
        }
    }
}
