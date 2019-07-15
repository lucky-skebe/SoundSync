// -----------------------------------------------------------------------
// <copyright file="PipeElementFactory.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using SharPipes.Pipes.Base.Attributes;
    using SharPipes.Pipes.Base.Helpers;

    /// <summary>
    /// Create elements give a factoryType name.
    ///
    /// These names can either be registered using the <see cref="ElementNameAttribute"/> or will be generated using the Classname.
    /// Classnames ending in Src, Sink, or Element will get these parts removed.
    /// </summary>
    public static class PipeElementFactory
    {
        private static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();

        static PipeElementFactory()
        {
            var parts = MEFExtensions.GetRegisteredTypes<IPipeElement>();

            foreach (var type in parts)
            {
                if (typeof(IPipeElement).IsAssignableFrom(type))
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        string name = GetName(type);
                        Types.Add(name, type);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of all known factoryType names.
        /// </summary>
        /// <returns>All known factoryType names.</returns>
        public static IReadOnlyCollection<string> GetFactoryTypes()
        {
            return Types.Keys;
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
        public static string GetName(Type type)
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
                string trimmed;

                if (TrimEnd(typeName, "element", out trimmed))
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

        /// <summary>
        /// Creates a new element given a factoryType name and the name the element should get.
        /// </summary>
        /// <param name="factoryType">The type of element to create.</param>
        /// <param name="name">the name the element should receive.</param>
        /// <returns>The newly created element or Null if the name was not found.</returns>
        public static IPipeElement? Make(string factoryType, string? name)
        {
            if (Types.ContainsKey(factoryType))
            {
                Type type = Types[factoryType];

                return Activator.CreateInstance(type, name) as IPipeElement;
            }
            else
            {
                return null;
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
