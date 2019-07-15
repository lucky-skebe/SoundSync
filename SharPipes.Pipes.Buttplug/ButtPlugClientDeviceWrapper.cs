// -----------------------------------------------------------------------
// <copyright file="ButtPlugClientDeviceWrapper.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Buttplug
{
    using System;
    using global::Buttplug.Client;
    using SharPipes.Pipes.Base.InteractionInfos;

    /// <summary>
    /// A Selectable Buttplugdevice.
    /// </summary>
    public class ButtPlugClientDeviceWrapper : Selectable<ButtplugClientDevice>, IEquatable<ButtPlugClientDeviceWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtPlugClientDeviceWrapper"/> class.
        /// </summary>
        /// <param name="device">The underlying buttplugdevice.</param>
        /// <param name="setSelectedDeviceCallback">A callback that is called each time the IsSelected Property Changes.</param>
        public ButtPlugClientDeviceWrapper(
            ButtplugClientDevice device,
            Action<Selectable<ButtplugClientDevice>, bool>? setSelectedDeviceCallback = null)
            : base(device, setSelectedDeviceCallback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtPlugClientDeviceWrapper"/> class.
        /// </summary>
        /// <param name="device">The underlying buttplugdevice.</param>
        /// <param name="selected">the initial selection state.</param>
        /// <param name="setSelectedDeviceCallback">A callback that is called each time the IsSelected Property Changes.</param>
        public ButtPlugClientDeviceWrapper(
            ButtplugClientDevice device,
            bool selected,
            Action<Selectable<ButtplugClientDevice>, bool>? setSelectedDeviceCallback = null)
            : base(device, selected, setSelectedDeviceCallback)
        {
        }

        /// <summary>
        /// Gets the name of the underlying buttplugdevice.
        /// </summary>
        /// <value>
        /// The name of the underlying buttplugdevice.
        /// </value>
        public string Name => this.Value.Name;

        /// <inheritdoc/>
        public bool Equals(ButtPlugClientDeviceWrapper other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value.Equals(other.Value);
        }
    }
}
