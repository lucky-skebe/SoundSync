using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CStreamer.Plugins.Buttplug;
using ReactiveUI;
using System;
using System.Linq;

namespace CStreamer.Plugins.Designer.Buttplug
{

    public class ButtPlugSettingsView : ReactiveUserControl<ButtplugSinkViewModel>
    {
        public ButtPlugSettingsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(() =>
            {
                return Enumerable.Empty<IDisposable>();
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
