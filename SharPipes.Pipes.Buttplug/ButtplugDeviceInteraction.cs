using Buttplug.Client;
using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Buttplug
{
    public class ButtplugDeviceInteraction : MultiSelectionInteraction<ButtPlugClientDeviceWrapper, ButtplugClientDevice>
    {
        public ButtplugDeviceInteraction() : base()
        {
        }
    }
}
