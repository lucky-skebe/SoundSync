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

namespace SoundSync.models
{
    

    public class Global: ViewModel, IDisposable
    {
        private readonly Thread background_thread;
        double sample_sum = 0.0f;
        int sample_count = 0;

        public Global()
        {

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            //this.AudioDevices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active).ToList();

            this.loopback = new WasapiLoopbackCapture();

            this.loopback.DataAvailable += DataAvailable;
            this.loopback.StartRecording();

            background_thread = new Thread(new ThreadStart(BackgroundWorker));
            background_thread.IsBackground = true;

            background_thread.Start();
        }

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

        


        #endregion

        #region Vibration Server


        #endregion

        

        public void Dispose()
        {
            loopback.StopRecording();
            StopScanning(null);
            Disconnect(null);
        }


    }
}
