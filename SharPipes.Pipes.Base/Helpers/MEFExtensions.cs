using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Collections.Generic;

namespace SharPipes.Pipes.Base.Helpers
{
    public static class MEFExtensions
    {
        static AggregateCatalog catalog;

        static MEFExtensions()
        {
            catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new DirectoryCatalog("."));
        }

        public static IEnumerable<Type> GetRegisteredTypes<T>()
        {

            return catalog.Parts.Where(IsPartOfType<T>).Select(part => ReflectionModelServices.GetPartType(part).Value);
        }

        private static bool IsPartOfType<T>(ComposablePartDefinition part)
        {
            return (part.ExportDefinitions.Any(
                def => def.Metadata.ContainsKey("ExportTypeIdentity") &&
                       def.Metadata["ExportTypeIdentity"].Equals(typeof(T).FullName)));
        }
    }
}
