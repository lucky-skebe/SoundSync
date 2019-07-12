using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Basic
{

    [Export(typeof(IPipeElement))]
    public class EnumerateElement : PipeTransform
    {
        public EnumerateElement(string? name = null) : base(name)
        {
            Src = new PipeSrcPad<double>(this, "src");
            Sink = new PipeSinkPad<IEnumerable<float>>(this, "sink", e =>
            {
                try
                {
                    foreach (var f in e)
                    {
                        Src.Push(f);
                    }
                } catch
                {

                }
            });
        }
        
        public PipeSinkPad<IEnumerable<float>> Sink
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


        public override string TypeName => "Unlist";
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

        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            if(Sink.Peer != null)
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
            return Enumerable.Empty<IPropertySetter>();
        }
    }
}
