using Avalonia;
using Avalonia.Markup.Xaml;
using CStreamer.Plugins.Designer.Base.ViewModels;

namespace CStreamer.Plugins.Designer.Base.Views
{
    public class FallbackSettingsView : ReactiveUserControl<FallbackSettingsViewModel>
    {
        public FallbackSettingsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
