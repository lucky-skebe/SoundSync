using Avalonia;
using Avalonia.Markup.Xaml;

namespace CStreamer.Designer.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
