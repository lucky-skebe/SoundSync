using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(PipeLine pipeline, IEnumerable<string> elementTypes)
        {
            this.ToolBar = new ToolBarViewModel(elementTypes);
            this.Pipeline = new PipelineViewModel(pipeline);
        }

        public ToolBarViewModel ToolBar { get; }

        public PipelineViewModel Pipeline { get; }
    }
}
