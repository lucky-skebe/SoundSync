using CStreamer.Plugins.Interfaces;
using Optional;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;

namespace CStreamer.Designer.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel(PipeLine pipeline, IEnumerable<string> elementTypes)
        {

            this.Notifications = new NotificationAreaViewModel();
            this.ToolBar = new ToolBarViewModel(elementTypes);
            this.Pipeline = new PipelineViewModel(pipeline);

            this.Play = ReactiveCommand.CreateFromTask(pipeline.Start);
            this.Stop = ReactiveCommand.CreateFromTask(pipeline.Stop);

            this.WhenActivated(disposables =>
            {
                this.Play.Subscribe(result => result.MatchNone( errors => { foreach (var error in errors) { this.Notifications.AddNotification(new Notification(error)); } })).DisposeWith(disposables);
                this.Stop.Subscribe(result => result.MatchNone(errors => { foreach (var error in errors) { this.Notifications.AddNotification(new Notification(error)); } })).DisposeWith(disposables);
            });

            
        }

        internal NotificationAreaViewModel Notifications { get; }
        public ToolBarViewModel ToolBar { get; }

        public PipelineViewModel Pipeline { get; }

        public ReactiveCommand<Unit, Option<State, IEnumerable<string>>> Play { get; }
        public ReactiveCommand<Unit, Option<State, IEnumerable<string>>> Pause { get; }
        public ReactiveCommand<Unit, Option<State, IEnumerable<string>>> Stop { get; }

    }
}
