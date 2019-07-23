using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.ViewModels;

namespace CStreamer.Designer.Avalonia.Views
{
    public class LinkView : ReactiveUserControl<LinkViewModel>
    {
        public LinkView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
