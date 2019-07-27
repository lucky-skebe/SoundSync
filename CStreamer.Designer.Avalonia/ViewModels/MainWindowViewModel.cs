using CStreamer.Plugins.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel(PipeLine pipeline, IEnumerable<string> elementTypes)
        {
            this.ToolBar = new ToolBarViewModel(elementTypes);
            this.Pipeline = new PipelineViewModel(pipeline);

            this.Play = ReactiveCommand.CreateFromTask(pipeline.Start);
            this.Stop = ReactiveCommand.CreateFromTask(pipeline.Stop);
        }

        public ToolBarViewModel ToolBar { get; }

        public PipelineViewModel Pipeline { get; }

        public ReactiveCommand Play { get; }
        public ReactiveCommand Pause { get; }
        public ReactiveCommand Stop { get; }

    }
}
