using Avalonia.Controls;
using CStreamer.Plugins.Buttplug;
using CStreamer.Plugins.Designer.Base;

namespace CStreamer.Plugins.Designer.Buttplug
{
    public class ButtplugDataTemplate : ElementSettingsDataTemplate<ButtplugSink>
    {
        public override IControl Build(ButtplugSink element)
        {
            return new ButtPlugSettingsView { DataContext = new ButtplugSinkViewModel(element) };
        }
    }
}
