using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System.Diagnostics;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Threading;

namespace SoundSync
{
    public enum ServerType
    {
        Embedded,
        Remote
    }

    public class Model : INotifyPropertyChanged, IDisposable
    {
        ButtplugClient client;
        private bool canConnect = true;
        private bool canDisconnect = true;

        private readonly Thread background_thread;
        double sample_sum = 0.0f;
        int sample_count = 0;

        public Model()
        {
            ConnectCommand = new DelegateCommand<object>(Connect, (param) => { return canConnect; });
            DisconnectCommand = new DelegateCommand<object>(Disconnect, (param) => { return canDisconnect; });
            StartScanningCommand = new DelegateCommand<object>(StartScanning, (_param) => { return CanStartScaning; });
            StopScanningCommand = new DelegateCommand<object>(StopScanning, (_param) => { return CanStopScaning; });

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            //this.AudioDevices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active).ToList();

            this.loopback = new WasapiLoopbackCapture();

            this.loopback.DataAvailable += DataAvailable;
            this.loopback.StartRecording();

            background_thread = new Thread(new ThreadStart(BackgroundWorker));
            background_thread.IsBackground = true;

            background_thread.Start();
        }

        #region Calculation

        private int _LowVolume = 0;

        public int LowVolume
        {
            get { return _LowVolume; }
            set
            {
                _LowVolume = value;
                OnPropertyChanged();
            }
        }

        private int _HighVolume = 10;

        public int HightVolume
        {
            get { return _HighVolume; }
            set
            {
                _HighVolume = value;
                OnPropertyChanged();
            }
        }

        private int _LowVibrate = 0;

        public int LowVibrate
        {
            get { return _LowVibrate; }
            set
            {
                _LowVibrate = value;
                OnPropertyChanged();
            }
        }

        private int _HighVibrate = 100;

        public int HighVibrate
        {
            get { return _HighVibrate; }
            set
            {
                _HighVibrate = value;
                OnPropertyChanged();
            }
        }

        double CalculateVibrate(double volume)
        {
            double volume_percent = (volume * 100);
            volume_percent = volume_percent < LowVolume ? LowVolume : (volume_percent > HightVolume ? HightVolume : volume_percent);
            int vol_spread = HightVolume - LowVolume;
            double rel_volume = (volume_percent - LowVolume) / vol_spread;
            double rel_vibration = (rel_volume * (HighVibrate - LowVibrate)) / 100;

            return rel_vibration + (LowVibrate / 100.0);
        }

        #endregion

        #region Background Work

        private double _UpdateInterval = 250;

        public double UpdateInterval
        {
            get { return _UpdateInterval; }
            set
            {
                _UpdateInterval = value;
                OnPropertyChanged();
            }
        }

        private double last_value = 0;   

        private void BackgroundWorker()
        {
            while (true)
            {
                Thread.Sleep((int)UpdateInterval);
                double avg = GetAverageSample();
                double vibration = CalculateVibrate(avg);

                if (SelectedDevice != null && last_value != vibration)
                {
                    SelectedDevice.InnerDevice.SendVibrateCmd(vibration);
                    last_value = vibration;
                }

            }
        }

        void AddSample(float sample)
        {
            lock (this)
            {
                sample_sum += Math.Abs(sample);
                sample_count++;
            }
        }

        public double GetAverageSample()
        {
            lock (this)
            {
                double avg = sample_sum / sample_count;
                sample_count = 0;
                sample_sum = 0.0f;
                return avg;
            }
        }

        #endregion

        #region Audio

        private readonly WasapiLoopbackCapture loopback;

        private void DataAvailable(object sender, WaveInEventArgs args)
        {
            var buffer = new WaveBuffer(args.Buffer);

            for (int index = 0; index < args.BytesRecorded / 4; index++)
            {
                var sample = buffer.FloatBuffer[index];
                AddSample(sample);
            }
        }

        private bool _IsRecording;

        public bool IsRecording
        {
            get { return _IsRecording; }
            set
            {
                _IsRecording = value;
                OnPropertyChanged();
                OnPropertyChanged("CanSwitchDevice");
                OnPropertyChanged("CanRecord");
                OnPropertyChanged("CanStop");

            }
        }

        public bool CanSwitchDevice
        {
            get
            {
                return !IsRecording;
            }
        }

        private bool _CanRecord;

        public bool CanRecord
        {
            get { return _CanRecord && !IsRecording; }
            set
            {
                _CanRecord = value;
                OnPropertyChanged();
            }
        }

        private bool _CanStop = true;

        public bool CanStop
        {
            get { return _CanStop && IsRecording; }
            set
            {
                _CanStop = value; OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        private List<MMDevice> _AudioDevices;

        public List<MMDevice> AudioDevices
        {
            get { return _AudioDevices; }
            set { _AudioDevices = value; }
        }

        private WasapiCapture _AudioDevice = new WasapiCapture();

        private MMDevice _SelectedAudioDevice;
        public MMDevice SelectedAudioDevice
        {
            get { return _SelectedAudioDevice; }
            set
            {
                if (_SelectedAudioDevice != null)
                {
                    _AudioDevice.RecordingStopped -= _AudioDevice_RecordingStopped;
                }
                _SelectedAudioDevice = value;
                OnPropertyChanged();
                if (_SelectedAudioDevice != null)
                {
                    CanRecord = true;
                    _AudioDevice = new WasapiCapture(_SelectedAudioDevice);
                    _AudioDevice.RecordingStopped += _AudioDevice_RecordingStopped;
                }
                else
                {
                    CanRecord = false;
                }
            }
        }

        private void _AudioDevice_RecordingStopped(object sender, StoppedEventArgs e)
        {
            IsRecording = false;
        }


        #endregion

        #region Vibration Server

        private string _ServerUrl = "ws://localhost:12345/buttplug";
        public DelegateCommand<object> StartScanningCommand { get; set; }
        public DelegateCommand<object> StopScanningCommand { get; set; }
        public DelegateCommand<object> ConnectCommand { get; set; }
        public DelegateCommand<object> DisconnectCommand { get; set; }
        public string ServerUrl { get { return _ServerUrl; } set { _ServerUrl = value; OnPropertyChanged(); } }

        private ServerType _ServerType = ServerType.Remote;

        public ServerType ServerType
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
            if (this.ServerType == ServerType.Embedded)
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

        public bool IsEmbeddedServer { get { return ServerType == ServerType.Embedded; } set { this.ServerType = ServerType.Embedded; } }
        public bool IsRemoteServer { get { return ServerType == ServerType.Remote; } set { this.ServerType = ServerType.Remote; } }
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

        #endregion

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var PropertyChanged = this.PropertyChanged;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            });
        }

        public void Dispose()
        {
            loopback.StopRecording();
            StopScanning(null);
            Disconnect(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
