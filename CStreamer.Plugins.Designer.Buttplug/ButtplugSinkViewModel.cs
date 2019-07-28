// -----------------------------------------------------------------------
// <copyright file="ButtplugSinkViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Buttplug
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using CStreamer.Plugins.Buttplug;
    using DynamicData;
    using DynamicData.Binding;
    using ReactiveUI;

    public class ButtplugSinkViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ButtplugSink sink;

        private ReadOnlyObservableCollection<ButtplugSinkDevice>? devices;

        public ButtplugSinkViewModel(ButtplugSink sink)
        {
            this.devices = null;
            this.sink = sink;
            this.Activator = new ViewModelActivator();


            this.WhenActivated((disposables) =>
            {
                sink.Devices.ToObservableChangeSet().ObserveOn(RxApp.MainThreadScheduler).Bind(out this.devices).Subscribe().DisposeWith(disposables);
            });


            this.StartScanning = ReactiveCommand.CreateFromTask(sink.StartScanning);
            this.StopScanning = ReactiveCommand.CreateFromTask(sink.StopScanning);
        }

        public ViewModelActivator Activator { get; }

        public ReadOnlyObservableCollection<ButtplugSinkDevice>? Devices => this.devices;

        public string ServerAddress
        {
            get => this.sink.ServerAddress;
            set
            {
                if (this.sink.ServerAddress != value)
                {
                    this.sink.ServerAddress = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ReactiveCommand StartScanning { get; }


        public ReactiveCommand StopScanning { get; }
    }
}
