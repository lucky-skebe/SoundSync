// -----------------------------------------------------------------------
// <copyright file="LoopBackSrc.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.NAudio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using global::NAudio.Wave;
    using SharPipes.Pipes.Base;

    /// <summary>
    /// Reads audiosamples from the Windows loopback device.
    /// </summary>
    [Export(typeof(IPipeElement))]
    public class LoopBackSrc : PipeSrc, IDisposable
    {
        private readonly WasapiLoopbackCapture loopback;

        private readonly PipeSrcPad<float> src;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopBackSrc"/> class.
        /// </summary>
        /// <param name="name">the name of the element.</param>
        public LoopBackSrc(string? name = null)
            : base(name)
        {
            this.loopback = new WasapiLoopbackCapture();
            this.loopback.DataAvailable += this.Loopback_DataAvailable;
            this.src = new PipeSrcPad<float>(this, "src");
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LoopBackSrc"/> class.
        /// </summary>
        ~LoopBackSrc()
        {
            // Finalizer calls Dispose(false)
            this.Dispose(false);
        }

        /// <inheritdoc/>
        public override string TypeName => "Audio Loopback";

        /// <inheritdoc/>
        public override PipeSrcPad<TValue>? GetSrcPad<TValue>(string name)
        {
            return null;
        }

        /// <inheritdoc/>
        public override GraphState Check()
        {
            if (this.src.IsLinked())
            {
                return GraphState.OK;
            }
            else
            {
                return GraphState.INCOMPLETE;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            return Enumerable.Empty<IPipeElement>();
        }

        /// <inheritdoc/>
        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            yield return this.src;
        }

        /// <inheritdoc/>
        public override IPipeSrcPad? GetSrcPad(string padName)
        {
            if (padName == null)
            {
                return null;
            }

            return padName.ToUpperInvariant() switch
            {
                "SRC" => this.src,
                _ => null
            };
        }

        /// <inheritdoc/>
        public override IPipeSinkPad? GetSinkPad(string toPad)
            => null;

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

            for (int index = 0; index < args.BytesRecorded / 4; index++)
            {
                var sample = buffer.FloatBuffer[index];

                this.src.Push(sample);
            }
        }
    }
}
