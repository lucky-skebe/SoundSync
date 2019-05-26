using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundSync.models
{
    public class Audio : ViewModel
    {

        private readonly WasapiLoopbackCapture loopback;

        

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
    }
}
