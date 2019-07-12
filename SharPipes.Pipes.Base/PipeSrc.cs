using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeSrc : PipeElement, IPipeSrc
    {
        public PipeSrc(string? name = null) : base(name)
        {
        }

        public override IEnumerable<IPipeSinkPad> GetSinkPads()
        {
            return Enumerable.Empty<IPipeSinkPad>();
        }

        public abstract PipeSrcPad<TValue>? GetSrc<TValue>(string name);

        public override abstract IEnumerable<IPipeSrcPad> GetSrcPads();
    }
}
