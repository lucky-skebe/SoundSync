// -----------------------------------------------------------------------
// <copyright file="DispatchedDeviceList.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.Helpers
{
    using System.Collections.ObjectModel;
    using System.Windows.Threading;
    using SharPipes.Pipes.Buttplug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    /// <summary>
    /// A wrapper around an <see cref="ObservableCollection{T}"/> that dispatched the changeEvents onto a given <see cref="Dispatcher"/>.
    /// </summary>
    public class DispatchedDeviceList : DispatchedObservableCollection<ButtPlugClientDeviceWrapper>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchedDeviceList"/> class.
        /// </summary>
        /// <param name="collection">The underlying List.</param>
        /// <param name="currentDispatcher">The dispatcher to dispatch the CollectionChangedEvents on.</param>
        public DispatchedDeviceList(ObservableCollection<ButtPlugClientDeviceWrapper> collection, Dispatcher currentDispatcher)
            : base(collection, currentDispatcher)
        {
        }
    }
}
