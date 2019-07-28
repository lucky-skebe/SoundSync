// -----------------------------------------------------------------------
// <copyright file="PadView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Markup.Xaml;
    using ReactiveUI;

    public class PadView : UserControl, IViewFor<SrcPadViewModel?>
    {
        public PadView()
        {
            this.InitializeComponent();
        }

        public SrcPadViewModel? ViewModel { get => this.DataContext as SrcPadViewModel; set => this.DataContext = value; }

        object IViewFor.ViewModel { get => this.DataContext; set => this.DataContext = value; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
