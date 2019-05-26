using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundSync.models
{
    public class Connection: ViewModel
    {
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

        static double CalculateVibrate(double volume, int lowVolume, int highVolume, int lowVibrate, int highVibrate)
        {
            double volume_percent = (volume * 100);
            volume_percent = volume_percent < lowVolume ? lowVolume : (volume_percent > highVolume ? highVolume : volume_percent);
            int vol_spread = highVolume - lowVolume;
            double rel_volume = (volume_percent - lowVolume) / vol_spread;
            double rel_vibration = (rel_volume * (highVibrate - lowVibrate)) / 100;

            return rel_vibration + (lowVibrate / 100.0);
        }

        private void DataAvailable(object sender, WaveInEventArgs args)
        {
            var buffer = new WaveBuffer(args.Buffer);

            for (int index = 0; index < args.BytesRecorded / 4; index++)
            {
                var sample = buffer.FloatBuffer[index];
                AddSample(sample);
            }
        }
    }
}
