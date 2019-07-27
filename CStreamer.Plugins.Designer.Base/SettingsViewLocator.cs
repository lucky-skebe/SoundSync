using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CStreamer.Plugins.Base;
using System.Collections.Generic;
using System.Linq;
using CStreamer.Base;
using System;
using System.Diagnostics;

namespace CStreamer.Plugins.Designer.Base
{
    public class SettingsViewLocator : IDataTemplate
    {
        public const int PRIORITY_FALLBACK = 1_000_000;
        public const int PRIORITY_LOWEST = 100;
        public const int PRIORITY_NORMAL = 50;
        public const int PRIORITY_HIGH = 10;
        public const int PRIORITY_FORCE = -1;

        public static SettingsViewLocator Instance { get; } = new SettingsViewLocator();

        private static readonly SortedList<int, List<IDataTemplate>> childTemplates = new SortedList<int, List<IDataTemplate>>();

        public bool SupportsRecycling => false;

        static SettingsViewLocator()
        {
            var templates = PluginCatalog
                .PluginTypes()
                .Where(type =>
                            {
                                var name = type.Name;
                                return
                                type.IsClass &&
                                !type.IsAbstract && 
                                typeof(ElementSettingsDataTemplate).IsAssignableFrom(type) &&
                                (type.GetGenericTypeImplementation(typeof(ElementSettingsDataTemplate<>)) != null) &&
                                type != typeof(FallbackDataTemplate);
                    });
            
            SettingsViewLocator.RegisterTemplate(new FallbackDataTemplate(), PRIORITY_FALLBACK);
            foreach(var templateType in templates)
            {
                SettingsViewLocator.RegisterTemplate((IDataTemplate)Activator.CreateInstance(templateType));
            }
        }

        private SettingsViewLocator()
        {
        }

        public static IEnumerable<(int, IDataTemplate)> GetMatchingTemplates(object data)
        {
            return childTemplates
                .SelectMany(keyValuePair => keyValuePair.Value
                    .Where(childTemplate => childTemplate.Match(data))
                    .Select(childTemplate => (keyValuePair.Key, childTemplate)));
        }

        public static void RegisterTemplate(IDataTemplate template, int priority = PRIORITY_NORMAL)
        {
            if (!childTemplates.ContainsKey(priority))
            {
                childTemplates.Add(priority, new List<IDataTemplate>());
            }

            childTemplates[priority].Add(template);
        }

        public IControl? Build(object data)
        {
            return GetMatchingTemplates(data).FirstOrDefault().Item2?.Build(data);
        }

        public bool Match(object data)
        {
            return GetMatchingTemplates(data).Any();
        }
    }
}
