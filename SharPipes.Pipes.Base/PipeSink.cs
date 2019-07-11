using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeSink : IPipeSink
    {
        public PipeSink(string? name = null)
        {
            this.Name = name ?? $"{PipeElementFactory.GetName(this.GetType())}-{Guid.NewGuid()}";
        }

        public Guid Id { get; }

        public abstract GraphState Check();

        public virtual IEnumerable<IInteraction> Interactions => Enumerable.Empty<IInteraction>();

        public abstract string TypeName { get; }
        public string Name { get; }

        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        public abstract PipeSinkPad<TValue>? GetSink<TValue>(string name);

        public abstract IEnumerable<IPipeSinkPad> GetSinkPads();

        public IEnumerable<IPipeSrcPad> GetSrcPads()
        {
            return Enumerable.Empty<IPipeSrcPad>();
        }

        public virtual Task Start()
        {
            return Task.CompletedTask;
        }

        public virtual Task Stop()
        {
            return Task.CompletedTask;
        }

        public abstract IEnumerable<PropertyValue> GetPropertyValues();
        public virtual bool SetPropertyValue(PropertyValue propvalue)
        {
            var setter = this.GetPropertySetters().FirstOrDefault(setter => setter.TrySetValue(propvalue));

            return setter != null;
        }

        protected abstract IEnumerable<IPropertySetter> GetPropertySetters();
        public abstract IPipeSrcPad? GetSrcPad(string fromPad);
        public abstract IPipeSinkPad? GetSinkPad(string toPad);
    }
}
