using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharPipes.Pipes.Base.InteractionInfos;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeElement : IPipeElement
    {
        public PipeElement(string? name = null)
        {
            this.Name = name ?? $"{PipeElementFactory.GetName(this.GetType())}-{Guid.NewGuid()}";
            this.CurrentState = State.Stopped;
        }

        public string Name { get; }
        public State CurrentState { get; private set; }
        public abstract string TypeName { get; }
        public virtual IEnumerable<IInteraction> Interactions => Enumerable.Empty<IInteraction>();

        public abstract GraphState Check();
        public abstract IEnumerable<IPipeElement> GetPrevNodes();
        public abstract IEnumerable<IPipeSinkPad> GetSinkPads();
        public abstract IEnumerable<IPipeSrcPad> GetSrcPads();

        public async Task GoToState(State newState)
        {
            var transitions = StateManager.GetTransitions(CurrentState, newState);

            foreach (var transition in transitions)
            {
                await ((this.CurrentState, transition) switch
                {
                    (State.Stopped, State.Ready) => this.TransitionStoppedReady(),
                    (State.Ready, State.Playing) => this.TransitionReadyPlaying(),
                    (State.Playing, State.Ready) => this.TransitionReadyStopped(),
                    (State.Ready, State.Stopped) => this.TransitionReadyStopped(),
                    _ => Task.CompletedTask,
                });
                this.CurrentState = transition;
            }
        }

        public virtual Task TransitionStoppedReady()
        {
            return Task.CompletedTask;
        }

        public virtual Task TransitionReadyPlaying()
        {
            return Task.CompletedTask;
        }

        public virtual Task TransitionPlayingReady()
        {
            return Task.CompletedTask;
        }

        public virtual Task TransitionReadyStopped()
        {
            return Task.CompletedTask;
        }
    }
}
