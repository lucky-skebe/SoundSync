using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Buttplug
{
    class ButtplugServerStateMachine
    {
        enum State
        {
            Disconnected,
            Connected,
            Scanning,
        }

        private State state;

        public ButtplugServerStateMachine()
        {
            this.state = State.Disconnected;
        }

        public bool CanConnect => this.state == State.Disconnected;
        public bool CanDisonnect => this.state == State.Connected;
        public bool CanStartScanning => this.state == State.Connected;
        public bool CanStopScanning => this.state == State.Scanning;

        internal bool ScanningFinished()
        {
            if(this.state == State.Scanning)
            {
                this.state = State.Connected;
                return true;
            }

            return false;
        }

        internal bool StartScanning()
        {
            if (this.state == State.Connected)
            {
                this.state = State.Scanning;
                return true;
            }

            return false;
        }

        internal bool StopScanning()
        {
            if (this.state == State.Scanning)
            {
                this.state = State.Connected;
                return true;
            }

            return false;
        }

        internal bool Connect()
        {
            if (this.state == State.Disconnected)
            {
                this.state = State.Connected;
                return true;
            }

            return false;
        }

        internal bool Disonnect()
        {
            if (this.state == State.Connected)
            {
                this.state = State.Disconnected;
                return true;
            }

            return false;
        }
    }
}
