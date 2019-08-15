// -----------------------------------------------------------------------
// <copyright file="ConvertElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Basic
{
    using System.Collections.Generic;
    using System.Linq;
    using CStreamer.Base;
    using CStreamer.Base.BaseElements;
    using CStreamer.Plugins.Base;

    /// <summary>
    /// An element to convert between different numeric types.
    /// </summary>
    public class ConvertElement : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertElement"/> class.
        /// </summary>
        /// <param name="name">The name of the element or null to autogenerate one.</param>
        public ConvertElement(string? name = null)
            : base(name)
        {
            var srcInt = new SrcPad<int>(this, "srcInt", false);
            var srcDouble = new SrcPad<double>(this, "srcDouble", false);
            var srcFloat = new SrcPad<float>(this, "srcFloat", false);

            this.Src = new CompositeSrcPad(
                this,
                "src",
                new List<ISrcPad>
                {
                    srcInt,
                    srcDouble,
                    srcFloat,
                },
                true);

            this.Sink = new CompositeSinkPad(
                this,
                "sink",
                new List<ISinkPad>
                {
                    new SinkPad<int>(
                        this,
                        "sinkInt",
                        (f) =>
                        {
                            srcInt.Push(f);
                            srcDouble.Push(f);
                            srcFloat.Push(f);
                        },
                        false),
                    new SinkPad<double>(
                        this,
                        "sinkDouble",
                        (f) =>
                        {
                            srcInt.Push((int)f);
                            srcDouble.Push(f);
                            srcFloat.Push((float)f);
                        },
                        false),
                    new SinkPad<float>(
                        this,
                        "sinkFloat",
                        (f) =>
                        {
                            srcInt.Push((int)f);
                            srcDouble.Push(f);
                            srcFloat.Push(f);
                        },
                        false),
                },
                true);
        }

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public CompositeSinkPad Sink
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public CompositeSrcPad Src
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPad> GetPads()
        {
            yield return this.Src;
            yield return this.Sink;
        }
    }
}
