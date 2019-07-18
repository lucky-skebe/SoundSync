// -----------------------------------------------------------------------
// <copyright file="TimedAVGElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Basic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using SharPipes.Pipes.Base;
    using SharPipes.Pipes.Base.InteractionInfos;

    /// <summary>
    /// Takes an average over all inputdata over a given amount of time.
    /// </summary>
    [Export(typeof(IElement))]
    public class TimedAVGElement : Element
    {
        private readonly Thread backgroundThread;

        private float accumulator = 0;
        private int count = 0;
        private bool running = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedAVGElement"/> class.
        /// </summary>
        /// <param name="name">the name of the element.</param>
        public TimedAVGElement(string? name = null)
            : base(name)
        {
            this.Src = new SrcPad<double>(this, "src", true);
            this.backgroundThread = new Thread(new ThreadStart(this.BackgroundWorker))
            {
                IsBackground = true,
            };
            this.Sink = new SinkPad<float>(this, "sink", (f) =>
            {
                lock (this)
                {
                    this.accumulator += Math.Abs(f);
                    this.count += 1;
                }
            }, true);
        }

        /// <summary>
        /// Gets the current average value.
        /// </summary>
        /// <value>
        /// The current average value.
        /// </value>
        public double AverageSample
        {
            get
            {
                lock (this)
                {
                    double avg = this.accumulator / this.count;
                    this.count = 0;
                    this.accumulator = 0.0f;
                    return avg;
                }
            }
        }

        ///// <inheritdoc/>
        //public override IEnumerable<IInteraction> Interactions
        //{
        //    get
        //    {
        //        yield return new IntParameterInteraction("Time (ms)", () => this.AVGMs, (value) => { this.AVGMs = value; });
        //    }
        //}

        /// <summary>
        /// Gets or sets the amount of millisecods between every average calculation.
        /// </summary>
        /// <value>
        /// The amount of millisecods between every average calculation.
        /// </value>
        public int AVGMs { get; set; } = 50;

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public SinkPad<float> Sink
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the one output srcpad this element has.
        /// </summary>
        /// <value>
        /// The one output srcpad this element has.
        /// </value>
        public SrcPad<double> Src
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        protected override Task TransitionReadyPlaying()
        {
            this.running = true;
            this.backgroundThread.Start();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        protected override Task TransitionPlayingReady()
        {
            this.running = false;
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            yield return new PropertyBinding<int>(() => this.AVGMs);
        }

        private void BackgroundWorker()
        {
            while (this.running)
            {
                Thread.Sleep(this.AVGMs);
                double avg = this.AverageSample;

                this.Src.Push(avg);
            }
        }

        public override IEnumerable<IPad> GetPads()
        {
            yield return this.Sink;
            yield return this.Src;
        }
    }
}
