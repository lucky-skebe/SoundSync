using CStreamer.Plugins.Base;
using CStreamer.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStreamer.Plugins.Basic
{
    class ConvertElement : Element
    {
        public ConvertElement(string? name = null) : base(name)
        {
            SrcInt = new SrcPad<int>(this, "srcInt", true);
            SrcDouble = new SrcPad<double>(this, "srcDouble", true);
            SrcFloat = new SrcPad<float>(this, "srcFloat", true);

            SinkInt = new SinkPad<int>(this, "sinkInt", (f) => {
                SrcInt.Push(f);
                SrcDouble.Push(f);
                SrcFloat.Push(f);
                }, true);
            SinkDouble = new SinkPad<double>(this, "sinkDouble", (f) => {
                SrcInt.Push((int)f);
                SrcDouble.Push(f);
                SrcFloat.Push((float)f);
            }, true);
            SinkFloat = new SinkPad<float>(this, "sinkFloat", (f) => {
                SrcInt.Push((int)f);
                SrcDouble.Push(f);
                SrcFloat.Push(f);
            }, true);
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


        public override IEnumerable<IPad> GetPads()
        {
            yield return SrcDouble;
            yield return SrcFloat;
            yield return SrcInt;
            yield return SinkDouble;
            yield return SinkFloat;
            yield return SinkInt;
        }

        public override IEnumerable<IPropertyBinding> GetPropertyBindings()
        {
            return Enumerable.Empty<IPropertyBinding>();
        }
    }
}
