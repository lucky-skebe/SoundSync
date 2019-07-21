using System;
using System.Collections.ObjectModel;
using Avalonia;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class PipelineViewModel : ViewModelBase
    {
        public PipelineViewModel()
        {
            this.Items = new ObservableCollection<ElementViewModel>();
        }

        public ObservableCollection<ElementViewModel> Items { get; }

        internal void CreateElement(string name, Point position)
        {
            var element = PipeElementFactory.Make(name, null);
            if(element != null)
            {
                this.Items.Add(new ElementViewModel(position.X, position.Y, element));
            }
        }
    }
}
