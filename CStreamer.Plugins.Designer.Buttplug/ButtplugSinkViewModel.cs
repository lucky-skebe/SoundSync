using Buttplug.Client;
using CStreamer.Plugins.Buttplug;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace CStreamer.Plugins.Designer.Buttplug
{
    public class ButtplugSinkViewModel : ReactiveObject, ISupportsActivation
    {
        private ButtplugSink sink;

        public ViewModelActivator Activator { get; }

        public ButtplugSinkViewModel(ButtplugSink sink)
        {
            this.sink = sink;
            this.Activator = new ViewModelActivator();
        }

        public ReadOnlyObservableCollection<ButtplugSinkDevice> Devices => sink.Devices;

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
    }
}
