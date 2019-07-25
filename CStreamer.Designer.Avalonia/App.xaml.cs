using Avalonia;
using Avalonia.Markup.Xaml;
using CStreamer.Plugins.Designer.Base;

namespace CStreamer.Designer.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            this.DataTemplates.Add(SettingsViewLocator.Instance);
            AvaloniaXamlLoader.Load(this);

        }
    }
}
