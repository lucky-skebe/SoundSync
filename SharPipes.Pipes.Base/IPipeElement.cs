using SharPipes.Pipes.Base.InteractionInfos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public interface IPipeElement
    {
        String Name { get; }

        Task GoToState(State newState);

        State CurrentState { get; }

        GraphState Check();

        string TypeName { get; }

        IEnumerable<IPipeElement> GetPrevNodes();

        IEnumerable<IInteraction> Interactions { get; }

        IEnumerable<IPipeSinkPad> GetSinkPads();

        IEnumerable<IPipeSrcPad> GetSrcPads();
    }
}
