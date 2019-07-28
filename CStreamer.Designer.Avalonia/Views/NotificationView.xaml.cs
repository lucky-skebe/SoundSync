// -----------------------------------------------------------------------
// <copyright file="NotificationView.xaml.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Views
{
    using System;
    using System.Linq;
    using CStreamer.Designer.Avalonia.ViewModels;
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;
    using ReactiveUI;

    public class NotificationView : ReactiveUserControl<NotificationViewModel>
    {
        public NotificationView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(Enumerable.Empty<IDisposable>);
            AvaloniaXamlLoader.Load(this);
        }
    }
}
