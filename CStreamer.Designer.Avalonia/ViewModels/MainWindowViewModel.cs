// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Disposables;
    using Optional;
    using ReactiveUI;

    using Notification = CStreamer.Designer.Avalonia.Notification;

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(PipeLine pipeline, IEnumerable<string> elementTypes)
        {
            this.Notifications = new NotificationAreaViewModel();
            this.ToolBar = new ToolBarViewModel(elementTypes);
            this.Pipeline = new PipelineViewModel(pipeline);

            this.Play = ReactiveCommand.CreateFromTask(() => pipeline.GoToState(State.Playing));
            this.Pause = ReactiveCommand.CreateFromTask(() => pipeline.GoToState(State.Ready));
            this.Stop = ReactiveCommand.CreateFromTask(() => pipeline.GoToState(State.Stopped));

            this.WhenActivated(disposables =>
            {
                this.Play.Subscribe(result => result.MatchNone(errors =>
                {
                    foreach (var error in errors)
                    {
                        this.Notifications.AddNotification(new Notification(error));
                    }
                })).DisposeWith(disposables);
                this.Pause.Subscribe(result => result.MatchNone(errors =>
                {
                    foreach (var error in errors)
                    {
                        this.Notifications.AddNotification(new Notification(error));
                    }
                })).DisposeWith(disposables);
                this.Stop.Subscribe(result => result.MatchNone(errors =>
                {
                    foreach (var error in errors)
                    {
                        this.Notifications.AddNotification(new Notification(error));
                    }
                })).DisposeWith(disposables);
            });
        }

        public ToolBarViewModel ToolBar { get; }

        public PipelineViewModel Pipeline { get; }

        public ReactiveCommand<Unit, Option<State, IEnumerable<string>>> Play { get; }

        public ReactiveCommand<Unit, Option<State, IEnumerable<string>>> Pause { get; }

        public ReactiveCommand<Unit, Option<State, IEnumerable<string>>> Stop { get; }

        internal NotificationAreaViewModel Notifications { get; }
    }
}
