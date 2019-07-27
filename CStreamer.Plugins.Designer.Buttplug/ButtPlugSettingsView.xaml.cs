using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CStreamer.Plugins.Buttplug;

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
            AvaloniaXamlLoader.Load(this);
        }
    }
}
