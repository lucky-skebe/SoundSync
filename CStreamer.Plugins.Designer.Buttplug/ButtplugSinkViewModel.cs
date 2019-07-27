using Buttplug.Client;
using CStreamer.Plugins.Buttplug;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData.List;
using DynamicData.Binding;
using System.Reactive.Disposables;

namespace CStreamer.Plugins.Designer.Buttplug
{
    public class ButtplugSinkViewModel : ReactiveObject, ISupportsActivation
    {
        private ButtplugSink sink;

        private ReadOnlyObservableCollection<ButtplugSinkDevice> devices;

        public ViewModelActivator Activator { get; }

        public ButtplugSinkViewModel(ButtplugSink sink)
        {
            this.sink = sink;
            this.Activator = new ViewModelActivator();


            this.WhenActivated((disposables) =>
            {
                sink.Devices.ToObservableChangeSet().ObserveOn(RxApp.MainThreadScheduler).Bind(out devices).Subscribe().DisposeWith(disposables);
            });

           
            this.StartScanning = ReactiveCommand.CreateFromTask(sink.StartScanning);
            this.StopScanning = ReactiveCommand.CreateFromTask(sink.StopScanning);
        }

        public ReadOnlyObservableCollection<ButtplugSinkDevice> Devices => this.devices;

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
