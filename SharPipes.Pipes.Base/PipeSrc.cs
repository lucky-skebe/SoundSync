using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeSrc : IPipeSrc
    {
        public PipeSrc() : this(Guid.NewGuid())
        {

        }

        public PipeSrc(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }

        public abstract GraphState Check();

        public virtual IEnumerable<IInteraction> Interactions => Enumerable.Empty<IInteraction>();

        public abstract string Name { get; }

        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        public IEnumerable<IPipeSinkPad> GetSinkPads()
        {
            return Enumerable.Empty<IPipeSinkPad>();
        }

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
    }
}
