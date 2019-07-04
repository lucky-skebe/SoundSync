using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using SharPipes.Pipes.Base;

namespace SharPipes.Pipes.Buttplug
{
    public class ButtplugSink : PipeSink
    {
        private ButtplugClient? _client;
        private double lastVal = 0;

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
        private string _ServerUrl = "ws://localhost:12345/buttplug";

        public string ServerUrl { get { return _ServerUrl; } set { _ServerUrl = value;  /* OnPropertyChanged(); */ } }

        public ButtplugSink()
        {
            Sink = new PipeSinkPad<double>(this, (f) => {
                if(lastVal != f)
                {
                    if (Client != null && Client.Connected)
                    {
                        foreach (var d in Client.Devices)
                        {
                            d.SendVibrateCmd(f);
                        }
                        lastVal = f;
                    }
                }
                
            });
        }

        public Task StartScanning()
        {
            if(this.Client != null)
            {
                return this.Client.StartScanningAsync();
            } else
            {
                return Task.CompletedTask;
            }
        }

        private void Client_ScanningFinished(object sender, EventArgs e)
        {
        }

        private void Client_DeviceRemoved(object sender, DeviceRemovedEventArgs e)
        {
        }

        private void Client_DeviceAdded(object sender, DeviceAddedEventArgs e)
        {
        }

        public PipeSinkPad<double> Sink;

        public override PipeSinkPad<TValue>? GetSink<TValue>(string name)
        {
            return null;
        }

        public override Task Start()
        {
            IButtplugClientConnector connector = new ButtplugWebsocketConnector(new Uri(ServerUrl));
            Client = new ButtplugClient("SoundSync", connector);
            return Client.ConnectAsync();
        }

        public override Task Stop()
        {
            if(Client != null)
            {
                return Client.DisconnectAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
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
    }
}
