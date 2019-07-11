using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using SharPipes.Pipes.Base;
using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.Pipes.Buttplug
{
    public class ButtplugSink : PipeSink
    {
        private ButtplugClient? _client;
        private double lastVal = 0;

        private readonly CommandInteraction connectInteraction;
        private readonly CommandInteraction startScanningInteraction;
        private readonly CommandInteraction stopScanningInteraction;
        private readonly CommandInteraction disconnectInteraction;
        private readonly ButtplugDeviceInteraction deviceInteraction;

        private readonly IList<ButtPlugClientDeviceWrapper> deviceList = new List<ButtPlugClientDeviceWrapper>();

        ButtplugClient? Client
        {
            get => _client;
            set
            {
                if(_client != null)
                {
                    _client.DeviceAdded -= Client_DeviceAdded;
                    _client.DeviceRemoved -= Client_DeviceRemoved;
                    _client.ScanningFinished -= Client_ScanningFinished;
                }
                _client = value;
                if (_client != null)
                {
                    _client.DeviceAdded += Client_DeviceAdded;
                    _client.DeviceRemoved += Client_DeviceRemoved;
                    _client.ScanningFinished += Client_ScanningFinished;
                }
            }
        }

        public string ServerUrl { get; set; } = "ws://localhost:12345/buttplug";

        public ButtplugSink()
        {
            Sink = new PipeSinkPad<double>(this, "sink", (f) => {
                if(lastVal != f)
                {
                    if (Client != null && Client.Connected )
                    {
                        foreach (var d in this.deviceList)
                        {
                            if(d.Selected)
                            {
                                d.Value.SendVibrateCmd(f);
                            }
                        }
                        lastVal = f;
                    }
                }
                
            });


            this.stateMachine = new ButtplugServerStateMachine();

            this.startScanningInteraction = new CommandInteraction("Start Scanning", async () => await this.StartScanning(), this.stateMachine.CanStartScanning);
            this.stopScanningInteraction = new CommandInteraction("Stop Scanning", async () => await this.StopScanning(), this.stateMachine.CanStopScanning);
            this.connectInteraction = new CommandInteraction("Connect", async () => await this.Connect(), this.stateMachine.CanConnect);
            this.disconnectInteraction = new CommandInteraction("Disconnect", async () => await this.Disconnect(), this.stateMachine.CanDisonnect);
            this.deviceInteraction = new ButtplugDeviceInteraction();
        }


        

        

        private void UpdateCommands()
        {
            this.startScanningInteraction.SetCanExecute(this.stateMachine.CanStartScanning);
            this.stopScanningInteraction.SetCanExecute(this.stateMachine.CanStopScanning);
            this.connectInteraction.SetCanExecute(this.stateMachine.CanConnect);
            this.disconnectInteraction.SetCanExecute(this.stateMachine.CanDisonnect);
        }

        public override string TypeName => "Buttplug";
        public async Task StartScanning()
        {
            if (this.Client == null)
            {
                return;
            }
            if (this.stateMachine.StartScanning())
            {
                this.startScanningInteraction.SetCanExecute(false);
                await this.Client.StartScanningAsync();


                UpdateCommands();
            }
        }

        public async Task StopScanning()
        {
            if (this.Client == null)
            {
                return;
            }

            if (this.stateMachine.StopScanning())
            {

                this.stopScanningInteraction.SetCanExecute(false);
                await this.Client.StopScanningAsync();

                UpdateCommands();
            }
        }

        public async Task Connect()
        {
            if (this.stateMachine.Connect())
            {
                IButtplugClientConnector connector = new ButtplugWebsocketConnector(new Uri(ServerUrl));
                Client = new ButtplugClient("SoundSync", connector);
                await Client.ConnectAsync();

                UpdateCommands();
            }
        }

        public async Task Disconnect()
        {
            if (this.Client == null)
            {
                return;
            }

            if(this.stateMachine.Disonnect())
            {
                await Client.DisconnectAsync();

                UpdateCommands();
            }
        }

        private void Client_ScanningFinished(object sender, EventArgs e)
        {
            this.stateMachine.ScanningFinished();
            UpdateCommands();
        }

        private void Client_DeviceRemoved(object sender, DeviceRemovedEventArgs e)
        {
            var device = new ButtPlugClientDeviceWrapper(e.Device);
            this.deviceList.Remove(device);
            this.deviceInteraction.Options.Remove(device);
        }

        private void Client_DeviceAdded(object sender, DeviceAddedEventArgs e)
        {
            var device = new ButtPlugClientDeviceWrapper(e.Device, (self, selected) => { if (selected) { self.Value.SendVibrateCmd(this.lastVal); } else { self.Value.SendVibrateCmd(0); } });
            this.deviceList.Add(device);
            this.deviceInteraction.Options.Add(device);
        }

        public PipeSinkPad<double> Sink;
        private readonly ButtplugServerStateMachine stateMachine;

        public override PipeSinkPad<TValue>? GetSink<TValue>(string name)
        {
            return null;
        }

        public override GraphState Check()
        {
            if(Sink.Edge != null)
            {
                return GraphState.OK;
            }
            else
            {
                return GraphState.INCOMPLETE;
            }
        }

        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            if (Sink.Edge != null)
            {
                yield return Sink.Edge.From.Parent;
            }
        }

        public override IEnumerable<IPipeSinkPad> GetSinkPads()
        {
            yield return Sink;
        }

        public override IEnumerable<PropertyValue> GetPropertyValues()
        {
            yield return new PropertyValue(nameof(ServerUrl), "string", ServerUrl);
        }

        public override IEnumerable<IInteraction> Interactions
        {
            get
            {
                yield return new StringParameterInteraction("ServerAddress:", () => this.ServerUrl, (serverUrl) => this.ServerUrl = serverUrl);
                yield return this.connectInteraction;
                yield return this.disconnectInteraction;
                yield return this.startScanningInteraction;
                yield return this.stopScanningInteraction;
                yield return this.deviceInteraction;
            }
        }
    }
}
