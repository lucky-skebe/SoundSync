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
    using System.IO;
    using System.Reactive;
    using System.Reactive.Disposables;
    using CStreamer.Base;
    using CStreamer.Designer.Avalonia.Helper;
    using global::Avalonia.Controls;
    using Newtonsoft.Json;
    using Optional;
    using ReactiveUI;

    using Notification = CStreamer.Designer.Avalonia.Notification;

    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(PipeLine pipeline, IEnumerable<string> elementTypes)
        {
            this.Notifications = new NotificationAreaViewModel();
            this.ToolBar = new ToolBarViewModel(elementTypes);
            this.Pipeline = new PipelineViewModel(pipeline, this.Notifications.AddNotification);

            this.Play = ReactiveCommand.CreateFromTask(() => pipeline.GoToState(State.Playing));
            this.Pause = ReactiveCommand.CreateFromTask(() => pipeline.GoToState(State.Ready));
            this.Stop = ReactiveCommand.CreateFromTask(() => pipeline.GoToState(State.Stopped));
            this.Save = ReactiveCommand.Create(async () =>
            {
                var sfd = new SaveFileDialog
                {
                    InitialFileName = "pipeline",
                    Filters = new List<FileDialogFilter>
                    {
                        new FileDialogFilter
                        {
                            Extensions = new List<string> { "json" },
                            Name = "Json",
                        },
                    },
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    DefaultExtension = "json",
                };

                var ret = await sfd.ShowAsync(global::Avalonia.Application.Current.MainWindow).ConfigureAwait(true);

                if (ret != null)
                {
                    DesignerPipelineDefinition pdef = this.Pipeline.SavePipeline();

                    var serialized = JsonConvert.SerializeObject(pdef, Formatting.Indented);

                    using StreamWriter fileStream = File.CreateText(ret);

                    fileStream.Write(serialized);
                }
            });
            this.Load = ReactiveCommand.CreateFromTask(async () =>
            {
                var ofd = new OpenFileDialog
                {
                    InitialFileName = "pipeline",
                    Filters = new List<FileDialogFilter>
                    {
                        new FileDialogFilter
                        {
                            Extensions = new List<string> { "json" },
                            Name = "Json",
                        },
                    },
                    AllowMultiple = false,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                };

                var ret = await ofd.ShowAsync(global::Avalonia.Application.Current.MainWindow).ConfigureAwait(true);

                if (ret.Length == 1)
                {
                    using Stream fileStream = File.OpenRead(ret[0]);
                    using var streamReader = new StreamReader(fileStream);
                    DesignerPipelineDefinition? pdef = JsonConvert.DeserializeObject<DesignerPipelineDefinition>(streamReader.ReadToEnd());
                    if (pdef != null)
                    {
                        this.Pipeline.LoadPipeline(pdef);
                    }
                }
            });

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

        public ReactiveCommand Save { get; }

        public ReactiveCommand Load { get; }

        internal NotificationAreaViewModel Notifications { get; }
    }
}
