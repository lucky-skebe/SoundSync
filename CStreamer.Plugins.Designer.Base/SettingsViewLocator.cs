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
    using CStreamer.Plugins.Base;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Templates;

    /// <summary>
    /// Picks one of the registered <see cref="IDataTemplate">DataTemplates</see> based on the registered Priority.
    /// </summary>
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

        /// <summary>
        /// Gets the Singelton SettingsViewLocator.
        /// </summary>
        /// <value>
        /// The Singelton SettingsViewLocator.
        /// </value>
        public static SettingsViewLocator Instance { get; } = new SettingsViewLocator();

        /// <inheritdoc/>
        public bool SupportsRecycling => false;

        /// <summary>
        /// Returns a list of all matching DataTemplates ordered by priority.
        /// </summary>
        /// <param name="data">The data to generate a DataTemplate for.</param>
        /// <returns>A list of all matching DataTemplates ordered by priority.</returns>
        public static IEnumerable<(int, IDataTemplate)> GetMatchingTemplates(object data)
        {
            return ChildTemplates
                .SelectMany(keyValuePair => keyValuePair.Value
                    .Where(childTemplate => childTemplate.Match(data))
                    .Select(childTemplate => (keyValuePair.Key, childTemplate)));
        }

        /// <summary>
        /// Registers a new IDataTemplate using a given <see cref="Priority"/>.
        /// </summary>
        /// <param name="template">The DataTemplate to register.</param>
        /// <param name="priority">the Priority with wich the DataTemlpate gets registered.</param>
        public static void RegisterTemplate(IDataTemplate template, int priority = Priority.NORMAL)
        {
            if (!ChildTemplates.ContainsKey(priority))
            {
                ChildTemplates.Add(priority, new List<IDataTemplate>());
            }

            ChildTemplates[priority].Add(template);
        }

        /// <inheritdoc/>
        public IControl? Build(object data)
        {
            return GetMatchingTemplates(data).FirstOrDefault().Item2?.Build(data);
        }

        /// <inheritdoc/>
        public bool Match(object data)
        {
            return GetMatchingTemplates(data).Any();
        }

        /// <summary>
        /// Default Priorities for DataTemplates.
        /// </summary>
        public static class Priority
        {
            /// <summary>
            /// If no other templates are found there is always one with the Fallback priority.
            /// </summary>
            public const int FALLBACK = 1_000_000;

            /// <summary>
            /// A low priority.
            /// Use this if your template can't use a restrictive type check.
            /// This way more specific ones can overwrite it.
            /// </summary>
            public const int LOWEST = 100;

            /// <summary>
            /// Default Priority if no priority is specified.
            /// Use this if your type restriction is normal like a class and all it's descendants.
            /// </summary>
            public const int NORMAL = 50;

            /// <summary>
            /// If your Template uses a really restricting condition like for example one specific type.
            /// </summary>
            public const int HIGH = 10;

            /// <summary>
            /// If you want to Force your datatemplate do be used. (Mainly during development).
            /// </summary>
            public const int FORCE = -1;
        }
    }
}
