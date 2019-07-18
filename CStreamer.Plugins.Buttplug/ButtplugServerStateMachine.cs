// -----------------------------------------------------------------------
// <copyright file="ButtplugServerStateMachine.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Buttplug
{
    internal class ButtplugServerStateMachine
    {
        private State state;

        public ButtplugServerStateMachine()
        {
            this.state = State.Disconnected;
        }

        internal enum State
        {
            /// <summary>
            /// Not connected to a server.
            /// </summary>
            Disconnected,

            /// <summary>
            /// Connected to a server.
            /// </summary>
            Connected,

            /// <summary>
            /// Currently scanning for devices.
            /// </summary>
            Scanning,
        }

        public bool CanConnect => this.state == State.Disconnected;

        public bool CanDisonnect => this.state == State.Connected;

        public bool CanStartScanning => this.state == State.Connected;

        public bool CanStopScanning => this.state == State.Scanning;

        internal bool ScanningFinished()
        {
            if (this.state == State.Scanning)
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
