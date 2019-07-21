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
        //private readonly CommandInteraction connectInteraction;
        //private readonly CommandInteraction startScanningInteraction;
        //private readonly CommandInteraction stopScanningInteraction;
        //private readonly CommandInteraction disconnectInteraction;
        //private readonly ButtplugDeviceInteraction deviceInteraction;

        //private readonly IList<ButtPlugClientDeviceWrapper> deviceList = new List<ButtPlugClientDeviceWrapper>();

        public SinkPad<double> Sink { get; }

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
            this.Sink = new SinkPad<double>(this, "sink", (f) =>
            {
                if (this.lastVal != f)
                {
                    if (this.Client != null && this.Client.Connected)
                    {
                        //foreach (var d in this.deviceList)
                        //{
                        //    if (d.Selected)
                        //    {
                        //        d.Value.SendVibrateCmd(f);
                        //    }
                        //}

                        this.lastVal = f;
                    }
                }
            }, true);

            this.stateMachine = new ButtplugServerStateMachine();

            //this.startScanningInteraction = new CommandInteraction("Start Scanning", async () => await this.StartScanning().ConfigureAwait(true), this.stateMachine.CanStartScanning);
            //this.stopScanningInteraction = new CommandInteraction("Stop Scanning", async () => await this.StopScanning().ConfigureAwait(true), this.stateMachine.CanStopScanning);
            //this.connectInteraction = new CommandInteraction("Connect", async () => await this.Connect().ConfigureAwait(true), this.stateMachine.CanConnect);
            //this.disconnectInteraction = new CommandInteraction("Disconnect", async () => await this.Disconnect().ConfigureAwait(true), this.stateMachine.CanDisonnect);
            //this.deviceInteraction = new ButtplugDeviceInteraction();
            this.selectedDeviceCache = new List<string>();
        }

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

        ///// <inheritdoc/>
        //public override IEnumerable<IInteraction> Interactions
        //{
        //    get
        //    {
        //        yield return new StringParameterInteraction("ServerAddress:", () => this.ServerAddress, (serverUrl) => this.ServerAddress = serverUrl);
        //        yield return this.connectInteraction;
        //        yield return this.disconnectInteraction;
        //        yield return this.startScanningInteraction;
        //        yield return this.stopScanningInteraction;
        //        yield return this.deviceInteraction;
        //    }
        //}

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
                //this.startScanningInteraction.SetCanExecute(false);
                await this.Client.StartScanningAsync().ConfigureAwait(true);

                this.UpdateCommands();
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
                //this.stopScanningInteraction.SetCanExecute(false);
                await this.Client.StopScanningAsync().ConfigureAwait(true);

                this.UpdateCommands();
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

                this.UpdateCommands();
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

                this.UpdateCommands();
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IPad> GetPads()
        {
            yield return this.Sink;
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

        /// <inheritdoc/>
        public override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            yield return new PropertyBinding<string>(() => this.ServerAddress);
            //yield return new PropertyBinding<List<string>>(
            //    "SelectedDevices",
            //    (s) => { this.selectedDeviceCache = s; },
            //    this.deviceList.Where(d => d.Selected).Select(w => GetDeviceId(w.Value)).ToList,
            //    this.ParseSelectedDeviceCache);
        }

        private static string GetDeviceId(ButtplugClientDevice device)
        {
            return device.Name;
        }

        private void UpdateCommands()
        {
            //this.startScanningInteraction.SetCanExecute(this.stateMachine.CanStartScanning);
            //this.stopScanningInteraction.SetCanExecute(this.stateMachine.CanStopScanning);
            //this.connectInteraction.SetCanExecute(this.stateMachine.CanConnect);
            //this.disconnectInteraction.SetCanExecute(this.stateMachine.CanDisonnect);
        }

        private void Client_ScanningFinished(object sender, EventArgs e)
        {
            this.stateMachine.ScanningFinished();
            this.UpdateCommands();
        }

        private void Client_DeviceRemoved(object sender, DeviceRemovedEventArgs e)
        {
            //var device = new ButtPlugClientDeviceWrapper(e.Device);
            //this.deviceList.Remove(device);
            //this.deviceInteraction.Options.Remove(device);
        }

        private void Client_DeviceAdded(object sender, DeviceAddedEventArgs e)
        {
            //bool selected = this.selectedDeviceCache.Remove(GetDeviceId(e.Device));

            //var device = new ButtPlugClientDeviceWrapper(e.Device, selected, (self, selected) =>
            //{
            //    if (selected)
            //    {
            //        self.Value.SendVibrateCmd(this.lastVal);
            //    }
            //    else
            //    {
            //        self.Value.SendVibrateCmd(0);
            //    }
            //});
            //this.deviceList.Add(device);
            //this.deviceInteraction.Options.Add(device);
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
