using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.ViewModels;
using ReactiveUI;
using System;
using System.Linq;

namespace CStreamer.Designer.Avalonia.Views
{
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
