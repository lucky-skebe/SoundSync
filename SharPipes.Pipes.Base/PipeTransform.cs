using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeTransform : IPipeTransform
    {
        public PipeTransform() : this(null)
        {

        }

        public PipeTransform(string? name)
        {
            this.Name = name ?? $"{PipeElementFactory.GetName(this.GetType())}-{Guid.NewGuid()}";
        }

        public abstract GraphState Check();

        public virtual IEnumerable<IInteraction> Interactions => Enumerable.Empty<IInteraction>();

        public abstract string TypeName { get; }

        public string Name { get; }

        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        public abstract PipeSinkPad<TValue>? GetSink<TValue>(string name);

        public abstract IEnumerable<IPipeSinkPad> GetSinkPads();

        public abstract PipeSrcPad<TValue>? GetSrc<TValue>(string name);

        public abstract IEnumerable<IPipeSrcPad> GetSrcPads();

        public virtual Task Start()
        {
            return Task.CompletedTask;
        }

        public virtual Task Stop()
        {
            return Task.CompletedTask;
        }

        public virtual IEnumerable<PropertyValue> GetPropertyValues()
        {
            return Enumerable.Empty<PropertyValue>();
        }
    }
}
