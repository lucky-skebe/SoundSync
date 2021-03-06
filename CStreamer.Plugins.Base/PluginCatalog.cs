﻿// -----------------------------------------------------------------------
// <copyright file="PluginCatalog.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Loads all the assemblies marked with the <see cref="PluginAssemblyAttribute"/>.
    ///
    /// It looks for assemblies in:
    /// <list type="number">
    /// <item>The current path (".")</item>
    /// <item>The plugin directory if it exists ("./plugins")(</item>
    /// <item>A directory accoring to the "CSTREAMER_PLUGIN_DIR" environment variable if it exists.</item>
    /// </list>
    /// </summary>
    public static class PluginCatalog
    {
        private static readonly List<Assembly> Plugins = new List<Assembly>();

        static PluginCatalog()
        {
            AddDirectory(".", false);
            AddDirectory("./plugins", true);

            var plugindir = System.Environment.GetEnvironmentVariable("CSTREAMER_PLUGIN_DIR");

            if (plugindir != null)
            {
                AddDirectory(plugindir, true);
            }
        }

        /// <summary>
        /// Returns a list of all exposed types inside the plugin assemblies.
        /// </summary>
        /// <returns>A list of all exposed types inside the plugin assemblies.</returns>
        public static IEnumerable<Type> PluginTypes()
        {
            return Plugins.SelectMany(plugin => plugin.ExportedTypes);
        }

        private static void AddDirectory(string path, bool searchSubdirs = true)
        {
            var dir = new System.IO.DirectoryInfo(path);

            if (!dir.Exists)
            {
                return;
            }

            var files = dir.GetFiles("*.dll", searchSubdirs ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                // TODO don't load all data of all assemblies using SystemReflection.Metadata
                var assembly = Assembly.LoadFrom(file.FullName);

                if (assembly.GetCustomAttribute<PluginAssemblyAttribute>() != null)
                {
                    Plugins.Add(assembly);
                }
            }
        }
    }
}
