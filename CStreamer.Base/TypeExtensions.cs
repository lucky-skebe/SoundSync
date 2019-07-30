// -----------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base
{
    using System;
    using System.Linq;

    /// <summary>
    /// Extension Methods for the Type type.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a Generic type implementation of a given type.
        ///
        /// So if instanceType inherits genericType{} it returns genericType{T}.
        /// </summary>
        /// <param name="instanceType">The type to check.</param>
        /// <param name="genericType">The generic type to check for as opengeneric.</param>
        /// <returns>The specific type implementation or null if it doesn't inherit that type.</returns>
        public static Type? GetGenericTypeImplementation(this Type instanceType, Type genericType)
        {
            while (instanceType != null)
            {
                if (instanceType.IsGenericType &&
                    instanceType.GetGenericTypeDefinition() == genericType)
                {
                    return instanceType;
                }

                instanceType = instanceType.BaseType;
            }

            return null;
        }

        /// <summary>
        /// Gets a Generic type interface of a given type.
        ///
        /// So if instanceType or any of it's parents implements genericType{} it returns genericType{T}.
        /// </summary>
        /// <param name="instanceType">The type to check.</param>
        /// <param name="genericType">The generic type to check for as opengeneric.</param>
        /// <returns>The specific type interface or null if it doesn't implement that type.</returns>
        public static Type? GetGenericInterfaceImplementation(this Type instanceType, Type genericType)
        {
            var interfaces = instanceType.FindInterfaces(FilterGenericType, genericType);

            return interfaces.FirstOrDefault();
        }

        private static bool FilterGenericType(Type typeObj, object criteriaObj)
        {
            return typeObj.IsGenericType && typeObj.GetGenericTypeDefinition() == (Type)criteriaObj;
        }
    }
}
