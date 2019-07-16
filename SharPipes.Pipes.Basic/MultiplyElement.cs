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
    public class MultiplyElement : TransformElement
    {
        public MultiplyElement(string? name = null)
            : base(name)
        {
            this.Src = new SrcPad<double>(this, "src");
            this.Sink = new SinkPad<double>(this, "sink", (f) => this.Src.Push(f * this.Multiplier));
        }

        public double Multiplier { get; set; } = 10;

        /// <inheritdoc/>
        public override IEnumerable<IInteraction> Interactions
        {
            get
            {
                yield return new DoubleParameterInteraction(
                    "Multiplier",
                    () => this.Multiplier,
                    (value) => { this.Multiplier = value; });
            }
        }

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
        public override string TypeName => "Multiply";

        /// <inheritdoc/>
        public override SinkPad<TValue>? GetSinkPad<TValue>(string name)
        {
            return null;
        }

        /// <inheritdoc/>
        public override SrcPad<TValue>? GetSrcPad<TValue>(string name)
        {
            return null;
        }

        /// <inheritdoc/>
        public override GraphState Check()
        {
            if (!this.Sink.IsLinked())
            {
                return GraphState.INCOMPLETE;
            }
            else if (!this.Src.IsLinked())
            {
                return GraphState.INCOMPLETE;
            }
            else
            {
                return GraphState.OK;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IElement> GetPrevNodes()
        {
            if (this.Sink.Peer != null)
            {
                yield return this.Sink.Peer.Parent;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<ISinkPad> GetSinkPads()
        {
            yield return this.Sink;
        }

        /// <inheritdoc/>
        public override IEnumerable<ISrcPad> GetSrcPads()
        {
            yield return this.Src;
        }

        /// <inheritdoc/>
        public override ISrcPad? GetSrcPad(string padName)
        {
            if (padName == null)
            {
                return null;
            }

            return padName.ToUpperInvariant() switch
            {
                "SRC" => this.Src,
                _ => null
            };
        }

        /// <inheritdoc/>
        public override ISinkPad? GetSinkPad(string padName)
        {
            if (padName == null)
            {
                return null;
            }

            return padName.ToUpperInvariant() switch
            {
                "SINK" => this.Sink,
                _ => null
            };
        }

        /// <inheritdoc/>
        protected override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            yield return new PropertyBinding<double>(() => this.Multiplier);
        }
    }
}
