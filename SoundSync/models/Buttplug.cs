using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace SoundSync.models
{
    public class Buttplug : ViewModel
    {
        public enum ServerTypes
        {
            Embedded,
            Remote
        }

        ButtplugClient client;
        private bool canConnect = true;
        private bool canDisconnect = true;
        private string _ServerUrl = "ws://localhost:12345/buttplug";
        public DelegateCommand<object> StartScanningCommand { get; set; }
        public DelegateCommand<object> StopScanningCommand { get; set; }
        public DelegateCommand<object> ConnectCommand { get; set; }
        public DelegateCommand<object> DisconnectCommand { get; set; }
        public string ServerUrl { get { return _ServerUrl; } set { _ServerUrl = value; OnPropertyChanged(); } }

        public Buttplug()
        {
            ConnectCommand = new DelegateCommand<object>(Connect, (param) => { return canConnect; });
            DisconnectCommand = new DelegateCommand<object>(Disconnect, (param) => { return canDisconnect; });
            StartScanningCommand = new DelegateCommand<object>(StartScanning, (_param) => { return CanStartScaning; });
            StopScanningCommand = new DelegateCommand<object>(StopScanning, (_param) => { return CanStopScaning; });
        }

        private ServerTypes _ServerType = ServerTypes.Remote;

        public ServerTypes ServerType
        {
            get { return _ServerType; }
            set
            {
                _ServerType = value;
                OnPropertyChanged();
                OnPropertyChanged("IsEmbeddedServer");
                OnPropertyChanged("IsRemoteServer");
            }
        }

        private async void Connect(object _parameter)
        {
            IButtplugClientConnector connector;
            if (this.ServerType == ServerTypes.Embedded)
            {
                connector = new ButtplugEmbeddedConnector("EmbeddedServer");
            }
            else
            {
                connector = new ButtplugWebsocketConnector(new Uri(ServerUrl));
            }
            client = new ButtplugClient("SoundSync", connector);
            client.DeviceAdded += Client_DeviceAdded;
            client.DeviceRemoved += Client_DeviceRemoved;
            client.ScanningFinished += Client_ScanningFinished;
            canConnect = false;
            await client.ConnectAsync();
            canConnect = true;
            Connected = true;
        }

        private async void Disconnect(object _parameter)
        {
            if (client != null)
            {
                await client.DisconnectAsync();
                Connected = false;
            }
        }

        public IEnumerable<Device> Devices
        {
            get
            {
                if (client?.Devices != null)
                {
                    foreach (var d in client?.Devices)
                    {
                        Console.WriteLine(d.Name);
                    }
                }

                return client?.Devices?.Select(d => new Device(d));
            }
        }

        public Device _SelectedDevice;
        public Device SelectedDevice
        {
            get { return _SelectedDevice; }
            set
            {
                if (_SelectedDevice != null)
                {
                    _SelectedDevice.InnerDevice.StopDeviceCmd();
                }
                _SelectedDevice = value;
            }
        }

        private async void StartScanning(object _parameter)
        {
            if (client != null)
            {
                CanStartScaning = false;


                await client.StartScanningAsync();
                CanStartScaning = true;
                ShowStartScaning = Visibility.Collapsed;
                ShowStopScaning = Visibility.Visible;
            }
        }

        private async void StopScanning(object _parameter)
        {
            if (client != null && client.Connected)
            {
                CanStartScaning = false;
                await client.StopScanningAsync();
                CanStartScaning = true;
                ShowStartScaning = Visibility.Visible;
                ShowStopScaning = Visibility.Collapsed;
            }
        }

        public bool IsEmbeddedServer { get { return ServerType == ServerTypes.Embedded; } set { this.ServerType = ServerTypes.Embedded; } }
        public bool IsRemoteServer { get { return ServerType == ServerTypes.Remote; } set { this.ServerType = ServerTypes.Remote; } }
        private bool _Connected;

        public bool Connected
        {
            get { return _Connected; }
            set
            {
                _Connected = value;
                OnPropertyChanged();
                OnPropertyChanged("ShowConnnect");
                OnPropertyChanged("ShowDisconnect");
            }
        }

        public Visibility ShowConnnect
        {
            get
            {
                return !Connected ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public Visibility ShowDisconnect
        {
            get
            {
                return Connected ? Visibility.Visible : Visibility.Hidden;
            }
        }


        private bool _CanStartScaning = true;

        public bool CanStartScaning
        {
            get { return _CanStartScaning; }
            set
            {
                _CanStartScaning = value;
                OnPropertyChanged();
                StartScanningCommand.FireCanExecuteChanged();
            }
        }

        private bool _CanStopScaning = true;

        public bool CanStopScaning
        {
            get { return _CanStopScaning; }
            set
            {
                _CanStopScaning = value;
                OnPropertyChanged();
                StopScanningCommand.FireCanExecuteChanged();
            }
        }

        private Visibility _ShowStartScaning = Visibility.Visible;

        public Visibility ShowStartScaning
        {
            get
            {
                return _ShowStartScaning;
            }
            set
            {
                _ShowStartScaning = value;
                OnPropertyChanged();
            }
        }

        private Visibility _ShowStopScaning;

        public Visibility ShowStopScaning
        {
            get
            {
                return _ShowStopScaning;
            }
            set
            {
                _ShowStopScaning = value;
                OnPropertyChanged();
            }
        }

        private void Client_ScanningFinished(object sender, EventArgs e)
        {
            ShowStartScaning = Visibility.Visible;
            ShowStopScaning = Visibility.Collapsed;
        }

        private void Client_DeviceRemoved(object sender, DeviceRemovedEventArgs e)
        {
            Console.WriteLine("Device Removed");
            OnPropertyChanged("Devices");
        }

        private void Client_DeviceAdded(object sender, DeviceAddedEventArgs e)
        {
            Console.WriteLine("Device Added");
            OnPropertyChanged("Devices");
        }
    }
}
