using System.Collections.ObjectModel;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class PipelineViewModel : ViewModelBase
    {
        public PipelineViewModel()
        {
            this.Items = new ObservableCollection<ElementViewModel>();
        }

        public ObservableCollection<ElementViewModel> Items { get; }
    }
}
