// -----------------------------------------------------------------------
// <copyright file="ButtplugSinkViewModel.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Designer.Buttplug
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using CStreamer.Plugins.Buttplug;
    using DynamicData;
    using DynamicData.Binding;
    using ReactiveUI;

    /// <summary>
    /// A ViewModel that uses a <see cref="ButtplugSink" /> as it's Model.
    /// </summary>
    public class ButtplugSinkViewModel : ReactiveObject, ISupportsActivation
    {
        private readonly ButtplugSink sink;

        private ReadOnlyObservableCollection<ButtplugSinkDevice>? devices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugSinkViewModel"/> class.
        /// </summary>
        /// <param name="sink">The <see cref="ButtplugSink"/> used as model.</param>
        public ButtplugSinkViewModel(ButtplugSink sink)
        {
            this.devices = null;
            this.sink = sink ?? throw new ArgumentNullException(nameof(sink));
            this.Activator = new ViewModelActivator();

            this.WhenActivated((disposables) =>
            {
                sink.Devices.ToObservableChangeSet().ObserveOn(RxApp.MainThreadScheduler).Bind(out this.devices).Subscribe().DisposeWith(disposables);
            });

            this.StartScanning = ReactiveCommand.CreateFromTask(sink.StartScanning);
            this.StopScanning = ReactiveCommand.CreateFromTask(sink.StopScanning);
        }

        /// <inheritdoc/>
        public ViewModelActivator Activator { get; }

        /// <summary>
        /// Gets a list of all Devices known to the Server and whether they should be controlled or not.
        /// </summary>
        /// <value>
        /// A list of all Devices known to the Server and whether they should be controlled or not.
        /// </value>
        public ReadOnlyObservableCollection<ButtplugSinkDevice>? Devices => this.devices;

        /// <summary>
        /// Gets or sets the Address of the Buttplug Server.
        /// </summary>
        /// <value>
        /// The Address of the Buttplug Server.
        /// </value>
        public string ServerAddress
        {
            get => this.sink.ServerAddress;
            set
            {
                if (this.sink.ServerAddress != value)
                {
                    this.sink.ServerAddress = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a Command to let the Server start scanning for new devices.
        /// </summary>
        /// <value>
        /// A Command to let the Server start scanning for new devices.
        /// </value>
        public ReactiveCommand StartScanning { get; }

        /// <summary>
        /// Gets a Command to let the Server stop scanning for new devices.
        /// </summary>
        /// <value>
        /// A Command to let the Server stop scanning for new devices.
        /// </value>
        public ReactiveCommand StopScanning { get; }
    }
}
