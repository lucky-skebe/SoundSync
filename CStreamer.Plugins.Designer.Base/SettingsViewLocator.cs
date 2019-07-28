// -----------------------------------------------------------------------
// <copyright file="SettingsViewLocator.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CStreamer.Base;
    using CStreamer.Plugins.Base;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Templates;

    public class SettingsViewLocator : IDataTemplate
    {
        private static readonly SortedList<int, List<IDataTemplate>> ChildTemplates = new SortedList<int, List<IDataTemplate>>();

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

            SettingsViewLocator.RegisterTemplate(new FallbackDataTemplate(), Priority.FALLBACK);
            foreach (var templateType in templates)
            {
                SettingsViewLocator.RegisterTemplate((IDataTemplate)Activator.CreateInstance(templateType));
            }
        }

        private SettingsViewLocator()
        {
        }

        public static SettingsViewLocator Instance { get; } = new SettingsViewLocator();

        public bool SupportsRecycling => false;

        public static IEnumerable<(int, IDataTemplate)> GetMatchingTemplates(object data)
        {
            return ChildTemplates
                .SelectMany(keyValuePair => keyValuePair.Value
                    .Where(childTemplate => childTemplate.Match(data))
                    .Select(childTemplate => (keyValuePair.Key, childTemplate)));
        }

        public static void RegisterTemplate(IDataTemplate template, int priority = Priority.NORMAL)
        {
            if (!ChildTemplates.ContainsKey(priority))
            {
                ChildTemplates.Add(priority, new List<IDataTemplate>());
            }

            ChildTemplates[priority].Add(template);
        }

        public IControl? Build(object data)
        {
            return GetMatchingTemplates(data).FirstOrDefault().Item2?.Build(data);
        }

        public bool Match(object data)
        {
            return GetMatchingTemplates(data).Any();
        }

        public static class Priority
        {
            public const int FALLBACK = 1_000_000;
            public const int LOWEST = 100;
            public const int NORMAL = 50;
            public const int HIGH = 10;
            public const int FORCE = -1;
        }
    }
}
