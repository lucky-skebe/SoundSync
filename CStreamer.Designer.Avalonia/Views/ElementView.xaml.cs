using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.Helper;
using CStreamer.Designer.Avalonia.ViewModels;

namespace CStreamer.Designer.Avalonia.Views
{
    public class ElementView : ReactiveUserControl<ElementViewModel>
    {
        public ElementView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
