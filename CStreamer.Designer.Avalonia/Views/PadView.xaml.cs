using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CStreamer.Designer.Avalonia.ViewModels;
using ReactiveUI;

namespace CStreamer.Designer.Avalonia.Views
{
    public class PadView : UserControl, IViewFor<SrcPadViewModel?>
    {
        public PadView()
        {
            this.InitializeComponent();
        }

        public SrcPadViewModel? ViewModel { get => this.DataContext as SrcPadViewModel; set => this.DataContext = value; }
        object IViewFor.ViewModel { get => this.DataContext; set => this.DataContext = value; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
