// -----------------------------------------------------------------------
// <copyright file="DispatchedDeviceListConverter.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System.Collections.ObjectModel;
    using System.Windows.Threading;
    using SharPipes.Pipes.Buttplug;

    /// <summary>
    /// Creates a <see cref="DispatchedObservableCollectionConverter{T}"/> of type <see cref="ButtPlugClientDeviceWrapper"/> from a <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public class DispatchedDeviceListConverter : DispatchedObservableCollectionConverter<ButtPlugClientDeviceWrapper>
    {
        /// <inheritdoc/>
        protected override DispatchedObservableCollection<ButtPlugClientDeviceWrapper> GetWrapper(ObservableCollection<ButtPlugClientDeviceWrapper> collection, Dispatcher dispatcher)
        {
            return new DispatchedDeviceList(collection, dispatcher);
        }
    }
}
