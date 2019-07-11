using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Basic
{
    public class EnumerateElement : PipeTransform
    {
        public EnumerateElement()
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
    }
}
