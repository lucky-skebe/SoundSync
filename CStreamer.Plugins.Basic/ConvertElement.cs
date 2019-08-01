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
    using CStreamer.Plugins.Base;
    using CStreamer.Plugins.Interfaces;

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
            this.SrcInt = new SrcPad<int>(this, "srcInt", false);
            this.SrcDouble = new SrcPad<double>(this, "srcDouble", false);
            this.SrcFloat = new SrcPad<float>(this, "srcFloat", false);

            this.SinkInt = new SinkPad<int>(
                this,
                "sinkInt",
                (f) =>
                {
                    this.SrcInt.Push(f);
                    this.SrcDouble.Push(f);
                    this.SrcFloat.Push(f);
                },
                false);
            this.SinkDouble = new SinkPad<double>(
                this,
                "sinkDouble",
                (f) =>
                {
                    this.SrcInt.Push((int)f);
                    this.SrcDouble.Push(f);
                    this.SrcFloat.Push((float)f);
                },
                false);
            this.SinkFloat = new SinkPad<float>(
                this,
                "sinkFloat",
                (f) =>
                {
                    this.SrcInt.Push((int)f);
                    this.SrcDouble.Push(f);
                    this.SrcFloat.Push(f);
                },
                false);
        }

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public SinkPad<int> SinkInt
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
        public SrcPad<int> SrcInt
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public SinkPad<double> SinkDouble
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
        public SrcPad<double> SrcDouble
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the one input sinkpad this element has.
        /// </summary>
        /// <value>
        /// The one output input this element has.
        /// </value>
        public SinkPad<float> SinkFloat
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
        public SrcPad<float> SrcFloat
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPad> GetPads()
        {
            yield return this.SrcDouble;
            yield return this.SrcFloat;
            yield return this.SrcInt;
            yield return this.SinkDouble;
            yield return this.SinkFloat;
            yield return this.SinkInt;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            return Enumerable.Empty<IPropertyBinding>();
        }
    }
}
