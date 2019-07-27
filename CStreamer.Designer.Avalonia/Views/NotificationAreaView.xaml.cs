using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CStreamer.Designer.Avalonia.Views
{
    public class NotificationAreaView : UserControl
    {
        public NotificationAreaView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
