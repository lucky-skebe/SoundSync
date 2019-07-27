using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStreamer.Base
{
    public static class TypeExtensions
    {
        private static bool FilterGenericType(Type typeObj, Object criteriaObj)
        {
            return typeObj.IsGenericType && typeObj.GetGenericTypeDefinition() == (Type)criteriaObj;
        }

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

    }
}
