// -----------------------------------------------------------------------
// <copyright file="LoopBackSrc.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.NAudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CStreamer.Base;
    using CStreamer.Base.BaseElements;
    using CStreamer.Plugins.Base;
    using global::NAudio.Wave;

    /// <summary>
    /// Reads audiosamples from the Windows loopback device.
    /// </summary>
    public class LoopBackSrc : Element, IDisposable
    {
        private readonly WasapiLoopbackCapture loopback;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopBackSrc"/> class.
        /// </summary>
        /// <param name="name">the name of the element.</param>
        public LoopBackSrc(string? name = null)
            : base(name)
        {
            this.loopback = new WasapiLoopbackCapture();
            this.loopback.DataAvailable += this.Loopback_DataAvailable;
            this.Src = new SrcPad<IEnumerable<float>>(this, "src", true);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LoopBackSrc"/> class.
        /// </summary>
        ~LoopBackSrc()
        {
            // Finalizer calls Dispose(false)
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the one output srcpad this element has.
        /// </summary>
        /// <value>
        /// The one output srcpad this element has.
        /// </value>
        public SrcPad<IEnumerable<float>> Src { get; }

        /// <inheritdoc/>
        public override IEnumerable<IPad> GetPads()
        {
            yield return this.Src;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected override Task TransitionReadyPlaying()
        {
            this.loopback.StartRecording();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        protected override Task TransitionPlayingReady()
        {
            this.loopback.StopRecording();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">True if child resources should be disposed as well.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.loopback != null)
                {
                    this.loopback.Dispose();
                }
            }
        }

        private void Loopback_DataAvailable(object sender, WaveInEventArgs args)
        {
            var buffer = new WaveBuffer(args.Buffer);

            this.Src.Push(buffer.FloatBuffer.Take(args.BytesRecorded / 4));
        }
    }
}
