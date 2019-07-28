// -----------------------------------------------------------------------
// <copyright file="ButtplugSinkDevice.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Buttplug
{
    using System;
    using global::Buttplug.Client;

    public class ButtplugSinkDevice : IEquatable<ButtplugSinkDevice>
    {
        public ButtplugSinkDevice(ButtplugClientDevice device, bool selected = true)
        {
            this.Device = device;
            this.Selected = selected;
        }

        public string Name => this.Device.Name;

        public ButtplugClientDevice Device { get; }

        public bool Selected { get; set; }

        public bool Equals(ButtplugSinkDevice other)
        {
            return this.Device == other.Device;
        }
    }
}
