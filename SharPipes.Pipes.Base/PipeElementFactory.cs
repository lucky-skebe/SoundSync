using SharPipes.Pipes.Base.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;

namespace SharPipes.Pipes.Base
{
    static class PipeElementFactory
    {
        private static readonly Dictionary<string, Type> types;

        public static string GetName(Type type)
        {
            ElementNameAttribute? attribute;
            if ((attribute = type.GetCustomAttribute<ElementNameAttribute>()) != null)
            {
                return attribute.Name;
            }
            else
            {
                var typeName = type.Name;
                string trimmed; 

                if(TrimEnd(typeName, "element", out trimmed))
                {
                    return trimmed;
                }else if (TrimEnd(typeName, "src", out trimmed))
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

        static PipeElementFactory()
        {
            types = new Dictionary<string, Type>();
            foreach (Assembly assembly in AssemblyLoadContext.Default.Assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IPipeElement).IsAssignableFrom(type))
                    {
                        if (type.IsClass && !type.IsAbstract)
                        {
                            string name = GetName(type);
                            types.Add(name, type);
                        }
                    }
                }
            }
        }

        public static IReadOnlyCollection<string> GetFactoryTypes()
        {
            return types.Keys;
        }

        public static IPipeElement? Make(string factoryType, string? name)
        {
            if(types.ContainsKey(factoryType))
            {
                Type type = types[factoryType];

                return Activator.CreateInstance(type, name) as IPipeElement; 
            }
            else
            {
                return null;
            }
        }
    }
}
