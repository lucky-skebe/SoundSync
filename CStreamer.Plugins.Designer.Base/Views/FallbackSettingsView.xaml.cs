// -----------------------------------------------------------------------
// <copyright file="FallbackSettingsView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Base.Views
{
    using CStreamer.Plugins.Designer.Base.ViewModels;
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;

    internal class FallbackSettingsView : ReactiveUserControl<FallbackSettingsViewModel>
    {
        public FallbackSettingsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
