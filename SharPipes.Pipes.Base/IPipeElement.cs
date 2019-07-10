using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public interface IPipeElement
    {
        public Guid Id { get; }
        Task Start();
        Task Stop();

        GraphState Check();

        string Name { get; }

        IEnumerable<IPipeElement> GetPrevNodes();

        IEnumerable<IInteraction> Interactions { get; }

        IEnumerable<IPipeSinkPad> GetSinkPads();

        IEnumerable<IPipeSrcPad> GetSrcPads();
    }
}
