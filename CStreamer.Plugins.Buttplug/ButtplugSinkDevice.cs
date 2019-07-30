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

    /// <summary>
    /// Contians a <see cref="ButtplugClientDevice"/> and a boolean indicating whether the contained device should be controlled.
    /// </summary>
    public class ButtplugSinkDevice : IEquatable<ButtplugSinkDevice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugSinkDevice"/> class.
        /// </summary>
        /// <param name="device">The contained <see cref="ButtplugClientDevice"/>.</param>
        /// <param name="selected">The initial selection.</param>
        public ButtplugSinkDevice(ButtplugClientDevice device, bool selected = true)
        {
            this.Device = device;
            this.Selected = selected;
        }

        /// <summary>
        /// Gets the Name of the contained <see cref="ButtplugClientDevice"/>.
        /// </summary>
        /// <value>
        /// The Name of the contained <see cref="ButtplugClientDevice"/>.
        /// </value>
        public string Name => this.Device.Name;

        /// <summary>
        /// Gets the contained <see cref="ButtplugClientDevice"/>.
        /// </summary>
        /// <value>
        /// The contained <see cref="ButtplugClientDevice"/>.
        /// </value>
        public ButtplugClientDevice Device { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the contained device should be controlled.
        /// </summary>
        /// <value>
        /// A value indicating whether gets or sets if the contained device should be controlled.
        /// </value>
        public bool Selected { get; set; }

        /// <inheritdoc/>
        public bool Equals(ButtplugSinkDevice other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Device == other.Device;
        }
    }
}
