using Buttplug.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundSync
{
    public class Device
    {
        private ButtplugClientDevice device;

        public Device(ButtplugClientDevice device)
        {
            this.device = device;
        }

        public string Name { get => device.Name; }
        public ButtplugClientDevice InnerDevice { get => device; }
    }
}
