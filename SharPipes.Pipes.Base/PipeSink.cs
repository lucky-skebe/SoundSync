using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeSink : IPipeSink
    {
        public PipeSink() : this(Guid.NewGuid())
        {

        }

        public PipeSink(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }

        public abstract GraphState Check();

        public virtual IEnumerable<ParameterInfo> DescribeParameters()
        {
            return Enumerable.Empty<ParameterInfo>();
        }

        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        public abstract PipeSinkPad<TValue>? GetSink<TValue>(string name);

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
