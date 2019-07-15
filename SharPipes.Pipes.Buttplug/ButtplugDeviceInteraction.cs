// -----------------------------------------------------------------------
// <copyright file="ButtplugDeviceInteraction.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Buttplug
{
    using global::Buttplug.Client;
    using SharPipes.Pipes.Base.InteractionInfos;

    /// <summary>
    /// A <see cref="MultiSelectionInteraction{TSelect, TValue}"/> that can select <see cref="ButtplugClientDevice"/>.
    /// </summary>
    public class ButtplugDeviceInteraction : MultiSelectionInteraction<ButtPlugClientDeviceWrapper, ButtplugClientDevice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugDeviceInteraction"/> class.
        /// </summary>
        public ButtplugDeviceInteraction()
            : base(string.Empty)
        {
        }
    }
}
