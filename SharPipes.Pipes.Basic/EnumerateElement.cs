// -----------------------------------------------------------------------
// <copyright file="EnumerateElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Basic
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using SharPipes.Pipes.Base;

    [Export(typeof(IElement))]
    public class EnumerateElement : TransformElement
    {
        public EnumerateElement(string? name = null)
            : base(name)
        {
            this.Src = new SrcPad<double>(this, "src");
            this.Sink = new SinkPad<IEnumerable<float>>(this, "sink", e =>
            {
                if (e == null)
                {
                    return;
                }

                foreach (var f in e)
                {
                    this.Src.Push(f);
                }
            });
        }

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public SinkPad<IEnumerable<float>> Sink
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
        public override string TypeName => "Unlist";

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
            return Enumerable.Empty<IPropertyBinding>();
        }
    }
}
