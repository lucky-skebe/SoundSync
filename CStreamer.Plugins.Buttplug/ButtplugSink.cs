// -----------------------------------------------------------------------
// <copyright file="ButtplugSink.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Buttplug
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CStreamer.Plugins.Base;
    using CStreamer.Plugins.Interfaces;
    using global::Buttplug.Client;
    using global::Buttplug.Client.Connectors.WebsocketConnector;
    using Newtonsoft.Json.Linq;
    using Optional;

    /// <summary>
    /// Sends the inputdata to selected Buttplug Devices as Vibrationcommands.
    /// </summary>
    public class ButtplugSink : Element
    {
        private readonly ObservableCollection<ButtplugSinkDevice> devices = new ObservableCollection<ButtplugSinkDevice>();

        private readonly ButtplugServerStateMachine stateMachine;

        private List<string> selectedDeviceCache;
        private ButtplugClient? client;
        private double lastVal = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtplugSink"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public ButtplugSink(string? name = null)
            : base(name)
        {
            this.Sink = new SinkPad<double>(
                this,
                "sink",
                (f) =>
                {
                    if (this.lastVal != f)
                    {
                        if (this.Client != null && this.Client.Connected)
                        {
                            foreach (var d in this.devices)
                            {
                                if (d.Selected)
                                {
                                    d.Device.SendVibrateCmd(f);
                                }
                            }

                            this.lastVal = f;
                        }
                    }
                },
                true
                );

            this.stateMachine = new ButtplugServerStateMachine();

            this.selectedDeviceCache = new List<string>();
        }

        /// <summary>
        /// Gets a list of selectable <see cref="ButtplugClientDevice">ButtplugClientDevices</see>.
        /// </summary>
        /// <value>
        /// A list of selectable <see cref="ButtplugClientDevice">ButtplugClientDevices</see>.
        /// </value>
        public ReadOnlyObservableCollection<ButtplugSinkDevice> Devices => new ReadOnlyObservableCollection<ButtplugSinkDevice>(this.devices);

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one input this element has.
        /// </value>
        public SinkPad<double> Sink { get; }

        /// <summary>
        /// Gets or sets the Client used to communicating with a Buttplug server.
        /// </summary>
        /// <value>
        /// The Client used to communicating with a Buttplug server.
        /// </value>
        public ButtplugClient? Client
        {
            get => this.client;
            set
            {
                if (this.client != null)
                {
                    this.client.DeviceAdded -= this.Client_DeviceAdded;
                    this.client.DeviceRemoved -= this.Client_DeviceRemoved;
                    this.client.ScanningFinished -= this.Client_ScanningFinished;
                }

                this.client = value;
                if (this.client != null)
                {
                    this.client.DeviceAdded += this.Client_DeviceAdded;
                    this.client.DeviceRemoved += this.Client_DeviceRemoved;
                    this.client.ScanningFinished += this.Client_ScanningFinished;
                }
            }
        }

        /// <summary>
        /// Gets or sets the address of the server to connecto to.
        /// </summary>
        /// <value>
        /// The address of the server to connecto to.
        /// </value>
        public string ServerAddress { get; set; } = "ws://localhost:12345/buttplug";

        /// <summary>
        /// Tell the Server to stop Scannign for new devices.
        /// </summary>
        /// <returns>A Task representing communication with the buttplug server.</returns>
        public async Task StartScanning()
        {
            if (this.Client == null)
            {
                return;
            }

            if (this.stateMachine.StartScanning())
            {
                await this.Client.StartScanningAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Tell the Server to stop Scannign for new devices.
        /// </summary>
        /// <returns>A Task representing communication with the buttplug server.</returns>
        public async Task StopScanning()
        {
            if (this.Client == null)
            {
                return;
            }

            if (this.stateMachine.StopScanning())
            {
                await this.Client.StopScanningAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Connect to the given Server.
        /// </summary>
        /// <returns>A Task representing connection process.</returns>
        public async Task Connect()
        {
            if (this.stateMachine.Connect())
            {
                IButtplugClientConnector connector = new ButtplugWebsocketConnector(new Uri(this.ServerAddress));
                this.Client = new ButtplugClient("SoundSync", connector);
                await this.Client.ConnectAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Disconnect to the given Server.
        /// </summary>
        /// <returns>A Task representing disconnection process.</returns>
        public async Task Disconnect()
        {
            if (this.Client == null)
            {
                return;
            }

            if (this.stateMachine.Disonnect())
            {
                await this.Client.DisconnectAsync().ConfigureAwait(true);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IPad> GetPads()
        {
            yield return this.Sink;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            yield return new PropertyBinding<string>(() => this.ServerAddress);
            yield return new PropertyBinding<List<string>>(
                "SelectedDevices",
                (s) => { this.selectedDeviceCache = s; },
                this.devices.Where(d => d.Selected).Select(w => GetDeviceId(w.Device)).ToList,
                this.ParseSelectedDeviceCache);
        }

        /// <inheritdoc/>
        protected override Task TransitionStoppedReady()
        {
            return this.Connect();
        }

        /// <inheritdoc/>
        protected override Task TransitionReadyStopped()
        {
            return this.Disconnect();
        }

        private static string GetDeviceId(ButtplugClientDevice device)
        {
            return device.Name;
        }

        private void Client_ScanningFinished(object sender, EventArgs e)
        {
            this.stateMachine.ScanningFinished();
        }

        private void Client_DeviceRemoved(object sender, DeviceRemovedEventArgs e)
        {
            var device = new ButtplugSinkDevice(e.Device);
            while (this.devices.Remove(device))
            {
            }
        }

        private void Client_DeviceAdded(object sender, DeviceAddedEventArgs e)
        {
            bool selected = this.selectedDeviceCache.Remove(GetDeviceId(e.Device));

            this.devices.Add(new ButtplugSinkDevice(e.Device, selected));
        }

        private Option<List<string>> ParseSelectedDeviceCache(object? value)
        {
            if (value is JArray array)
            {
                List<string> ret = new List<string>();
                foreach (var entry in array)
                {
                    if (entry.Type != JTokenType.String)
                    {
                        return Option.None<List<string>>();
                    }

                    ret.Add(entry.ToObject<string>());
                }

                return Option.Some<List<string>>(ret);
            }

            return Option.None<List<string>>();
        }
    }
}
