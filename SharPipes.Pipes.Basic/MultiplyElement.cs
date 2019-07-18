// -----------------------------------------------------------------------
// <copyright file="MultiplyElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Basic
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using SharPipes.Pipes.Base;
    using SharPipes.Pipes.Base.InteractionInfos;

    [Export(typeof(IElement))]
    public class MultiplyElement : Element
    {
        public MultiplyElement(string? name = null)
            : base(name)
        {
            this.Src = new SrcPad<double>(this, "src", true);
            this.Sink = new SinkPad<double>(this, "sink", (f) => this.Src.Push(f * this.Multiplier), true);
        }

        public double Multiplier { get; set; } = 10;

        /// <inheritdoc/>
        //public override IEnumerable<IInteraction> Interactions
        //{
        //    get
        //    {
        //        yield return new DoubleParameterInteraction(
        //            "Multiplier",
        //            () => this.Multiplier,
        //            (value) => { this.Multiplier = value; });
        //    }
        //}

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
