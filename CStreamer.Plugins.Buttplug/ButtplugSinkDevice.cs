using Buttplug.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Buttplug
{
    public class ButtplugSinkDevice : IEquatable<ButtplugSinkDevice>
    {
        public ButtplugSinkDevice(ButtplugClientDevice device, bool selected = false)
        {
            this.Device = device;
            this.Selected = selected;
        }

        public ButtplugClientDevice Device { get; }
        public bool Selected { get; }

        public bool Equals(ButtplugSinkDevice other)
        {
            return this.Device == other.Device;
        }
    }
}
