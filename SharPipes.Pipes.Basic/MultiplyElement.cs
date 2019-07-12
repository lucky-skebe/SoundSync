using SharPipes.Pipes.Base;
using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Basic
{
    public class MultiplyElement : PipeTransform 
    {
        public MultiplyElement(string? name = null) : base(name)
        {
            Src = new PipeSrcPad<double>(this, "src");
            Sink = new PipeSinkPad<double>(this, "sink", (f) => Src.Push(f * this.Multiplier));
        }

        private double _Multiplier = 10;

        public double Multiplier
        {
            get { return _Multiplier; }
            set { _Multiplier = value; }
        }


        public override IEnumerable<IInteraction> Interactions
        {
            get {
                yield return new DoubleParameterInteraction("Multiplier",
                    () => this.Multiplier, 
                    (value) => { this.Multiplier = value; }
                    );
            }
            
        }

        public PipeSinkPad<double> Sink
        {
            get;
            set;
        }

        public PipeSrcPad<double> Src
        {
            get;
            set;
        }

        public override PipeSinkPad<TValue>? GetSink<TValue>(string name)
        {
            return null;
        }

        public override PipeSrcPad<TValue>? GetSrc<TValue>(string name)
        {
            return null;
        }

        public override GraphState Check()
        {
            if (!Sink.IsLinked)
            {
                return GraphState.INCOMPLETE;
            }
            else if (!Src.IsLinked)
            {
                return GraphState.INCOMPLETE;
            }
            else
            {
                return GraphState.OK;
            }
        }


        public override string TypeName => "Multiply";
        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            if (Sink.Peer != null)
            {
                yield return Sink.Peer.Parent;
            }
        }

        public override IEnumerable<IPipeSinkPad> GetSinkPads()
        {
            yield return Sink;
        }

        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            yield return Src;
        }

        public override IEnumerable<PropertyValue> GetPropertyValues()
        {
            yield return new PropertyValue(nameof(Multiplier), Multiplier);
        }

        public override IPipeSrcPad? GetSrcPad(string fromPad)
            => fromPad.ToLower() switch
            {
                "src" => this.Src,
                _ => null
            };

        public override IPipeSinkPad? GetSinkPad(string toPad)
            => toPad.ToLower() switch
            {
                "sink" => this.Sink,
                _ => null
            };

        protected override IEnumerable<IPropertySetter> GetPropertySetters()
        {
            yield return new PropertySetter<double>(() => this.Multiplier);
        }
    }
}
