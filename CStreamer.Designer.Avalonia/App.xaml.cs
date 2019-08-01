// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia
{
    using CStreamer.Plugins.Designer.Base;
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;

    internal class App : Application
    {
        public override void Initialize()
        {
            this.DataTemplates.Add(SettingsViewLocator.Instance);
            AvaloniaXamlLoader.Load(this);
        }
    }
}
