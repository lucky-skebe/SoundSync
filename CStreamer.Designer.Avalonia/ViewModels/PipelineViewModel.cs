using System;
using System.Collections.ObjectModel;
using Avalonia;
using CStreamer.Base;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class PipelineViewModel : ViewModelBase
    {
        public PipelineViewModel()
        {
            this.Items = new ObservableCollection<ICStreamerViewModel>();
        }

        public ObservableCollection<ICStreamerViewModel> Items { get; }

        internal void CreateElement(string name, Point position)
        {
            var element = PipeElementFactory.Make(name, null);
            if(element != null)
            {
                var elementVM = new ElementViewModel(position.X, position.Y, element);
                this.Items.Add(elementVM);
                int i = 0;
                foreach(var srcPad in element.GetSrcPads())
                {
                    this.Items.Add(new PadViewModel(elementVM, PadViewModel.PadType.Src, i++));
                }

                i = 0;
                foreach (var srcPad in element.GetSinkPads())
                {
                    this.Items.Add(new PadViewModel(elementVM, PadViewModel.PadType.Sink, i++));
                }

            }
        }
    }
}
