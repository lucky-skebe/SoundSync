using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class TransformElement<TFrom, TTo> : IPipeTransform
    {
        public Guid Id { get; }

        public TransformElement() : this(Guid.NewGuid())
        {
        }

        public TransformElement(Guid id)
        {
            this.Id = id;
        }

        public abstract IEnumerable<IInteraction> Interactions { get; }

        public abstract string Name { get; }

        public PipeSinkPad<TValue>? GetSink<TValue>(string name)
        {
            return null;
        }

        public PipeSrcPad<TValue>? GetSrc<TValue>(string name)
        {
            return null;
        }

        protected abstract void Convert(TFrom fromValue, TTo toValue);

        public abstract Task Start();

        public abstract Task Stop();

        public abstract GraphState Check();

        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        public abstract IEnumerable<IPipeSinkPad> GetSinkPads();

        public abstract IEnumerable<IPipeSrcPad> GetSrcPads();
    }
}
