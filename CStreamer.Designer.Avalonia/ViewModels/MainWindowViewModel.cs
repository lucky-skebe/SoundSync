using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IEnumerable<string> elements)
        {
            this.ToolBar = new ToolBarViewModel(elements);
            this.Pipeline = new PipelineViewModel();
        }

        public ToolBarViewModel ToolBar { get; }

        public PipelineViewModel Pipeline { get; }
    }
}
