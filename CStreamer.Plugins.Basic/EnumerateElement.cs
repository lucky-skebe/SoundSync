// -----------------------------------------------------------------------
// <copyright file="EnumerateElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Basic
{
    using System.Collections.Generic;
    using System.Linq;
    using CStreamer.Plugins.Base;
    using CStreamer.Plugins.Interfaces;

    public class EnumerateElement : Element
    {
        public EnumerateElement(string? name = null)
            : base(name)
        {
            this.Src = new SrcPad<float>(this, "src", true);
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
            }, true);
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
        public SrcPad<float> Src
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
            return Enumerable.Empty<IPropertyBinding>();
        }
    }
}
