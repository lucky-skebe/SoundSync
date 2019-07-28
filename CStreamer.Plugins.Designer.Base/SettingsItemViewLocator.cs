// -----------------------------------------------------------------------
// <copyright file="SettingsItemViewLocator.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base
{
    using System;
    using CStreamer.Plugins.Designer.Base.ViewModels.Settings;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Templates;

    internal class SettingsItemViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            Type type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + data.GetType().Name };
            }
        }

        public bool Match(object data)
        {
            return data is ISettingViewModel;
        }
    }
}
