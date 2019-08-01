// -----------------------------------------------------------------------
// <copyright file="MultiplyElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Basic
{
    using System.Collections.Generic;
    using CStreamer;
    using CStreamer.Plugins.Base;
    using CStreamer.Plugins.Interfaces;

    /// <summary>
    /// An Element that multiplies the input by a fixed value.
    /// </summary>
    public class MultiplyElement : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyElement"/> class.
        /// </summary>
        /// <param name="name">The Name of hte element.</param>
        public MultiplyElement(string? name = null)
            : base(name)
        {
            this.Src = new SrcPad<double>(this, "src", true);
            this.Sink = new SinkPad<double>(this, "sink", (f) => this.Src.Push(f * this.Multiplier), true);
        }

        /// <summary>
        /// Gets or sets the amount the input gets multiplied by.
        /// </summary>
        /// <value>
        /// the amount the input gets multiplied by.
        /// </value>
        public double Multiplier { get; set; } = 10;

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public SinkPad<double> Sink
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
        public override IEnumerable<IPad> GetPads()
        {
            yield return this.Sink;
            yield return this.Src;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            yield return new PropertyBinding<double>(() => this.Multiplier);
        }
    }
}
