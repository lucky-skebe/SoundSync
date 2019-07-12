using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
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

        public override IEnumerable<PropertyValue> GetPropertyValues()
        {
            return Enumerable.Empty<PropertyValue>();
        }

        protected override IEnumerable<IPropertySetter> GetPropertySetters()
        {
            return Enumerable.Empty<IPropertySetter>();
        }
    }
}
