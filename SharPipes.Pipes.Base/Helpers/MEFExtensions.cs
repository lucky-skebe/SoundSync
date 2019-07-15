// -----------------------------------------------------------------------
// <copyright file="MEFExtensions.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.ComponentModel.Composition.ReflectionModel;
    using System.Linq;

    internal static class MEFExtensions
    {
        private static readonly AggregateCatalog Catalog = new AggregateCatalog();

        static MEFExtensions()
        {
            #pragma warning disable CA2000 // Dispose objects before losing scope
            var directoryCatalog = new DirectoryCatalog(".");
            #pragma warning restore CA2000 // Dispose objects before losing scope
            Catalog.Catalogs.Add(directoryCatalog);
        }

        public static IEnumerable<Type> GetRegisteredTypes<T>()
        {
            return Catalog.Parts.Where(IsPartOfType<T>).Select(part => ReflectionModelServices.GetPartType(part).Value);
        }

        private static bool IsPartOfType<T>(ComposablePartDefinition part)
        {
            return part.ExportDefinitions.Any(
                def => def.Metadata.ContainsKey("ExportTypeIdentity") &&
                       def.Metadata["ExportTypeIdentity"].Equals(typeof(T).FullName));
        }
    }
}
