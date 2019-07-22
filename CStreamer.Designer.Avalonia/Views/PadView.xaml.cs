using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CStreamer.Designer.Avalonia.Views
{
    public class PadView : UserControl
    {
        public PadView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
