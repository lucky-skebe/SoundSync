﻿// -----------------------------------------------------------------------
// <copyright file="PipeElementFactory.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer
{
    using System;
    using System.Collections.Generic;
    using CStreamer.Helpers;
    using CStreamer.Plugins.Interfaces;

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
            var parts = MEFExtensions.GetRegisteredTypes<IElement>();

            foreach (var type in parts)
            {
                if (typeof(IElement).IsAssignableFrom(type))
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        string name = CStreamer.IElementExtensions.GetElementName(type);
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
        /// Creates a new element given a factoryType name and the name the element should get.
        /// </summary>
        /// <param name="factoryType">The type of element to create.</param>
        /// <param name="name">the name the element should receive.</param>
        /// <returns>The newly created element or Null if the name was not found.</returns>
        public static IElement? Make(string factoryType, string? name)
        {
            if (Types.ContainsKey(factoryType))
            {
                Type type = Types[factoryType];

                return Activator.CreateInstance(type, name) as IElement;
            }
            else
            {
                return null;
            }
        }
    }
}
