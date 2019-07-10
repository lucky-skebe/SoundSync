using Buttplug.Client;
using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Buttplug
{
    public class ButtPlugClientDeviceWrapper : Selectable<ButtplugClientDevice>, IEquatable<ButtPlugClientDeviceWrapper>
    {

        public ButtPlugClientDeviceWrapper(ButtplugClientDevice device, Action<Selectable<ButtplugClientDevice>, bool>? setSelectedDeviceCallback = null) : base(device, setSelectedDeviceCallback)
        {
        }

        public string Name => this.Value.Name;

        public bool Equals(ButtPlugClientDeviceWrapper other)
        {
            return this.Value.Equals(other.Value);
        }
    }
}
