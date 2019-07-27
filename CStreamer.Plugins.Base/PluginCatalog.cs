using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.IO;
using System.Diagnostics;

namespace CStreamer.Plugins.Base
{
    public static class PluginCatalog
    {
        private static List<Assembly> plugins = new List<Assembly>();

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
                // TODO don't load all data of all assemblies

                var assembly = Assembly.LoadFrom(file.FullName);

                if (assembly.GetCustomAttribute<PluginAssemblyAttribute>() != null)
                {
                    plugins.Add(assembly);
                }
            }
        }

        public static IEnumerable<Type> PluginTypes ()
        {
            return plugins.SelectMany(plugin => plugin.DefinedTypes);
        }
    }
}
