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

    public static class TypeExtensions
    {
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
