// -----------------------------------------------------------------------
// <copyright file="SettingsView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using System;
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Data;
    using global::Avalonia.Markup.Xaml;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes.
    internal class SettingsView : UserControl
#pragma warning enable CA1812 // Avoid uninstantiated internal classes.
    {
        public SettingsView()
        {
            this.InitializeComponent();
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
