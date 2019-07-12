using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeSink : PipeElement, IPipeSink
    {
        public PipeSink(string? name = null) : base(name)
        {
        }

        public abstract PipeSinkPad<TValue>? GetSink<TValue>(string name);

        public override abstract IEnumerable<IPipeSinkPad> GetSinkPads();

        public override IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            return Enumerable.Empty<IPipeSrcPad>();
        }

        protected override IEnumerable<IPropertySetter> GetPropertySetters()
        {
            return Enumerable.Empty<IPropertySetter>();
        }
    }
}
