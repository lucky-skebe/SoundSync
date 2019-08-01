// -----------------------------------------------------------------------
// <copyright file="ElementView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes.
    internal class ElementView : ReactiveUserControl<ElementViewModel>
#pragma warning enable CA1812 // Avoid uninstantiated internal classes.
    {
        public ElementView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
