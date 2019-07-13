using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;

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
                    (State.Playing, State.Ready) => this.TransitionPlayingReady(),
                    (State.Ready, State.Stopped) => this.TransitionReadyStopped(),
                    _ => Task.CompletedTask,
                });
                this.CurrentState = transition;
            }
        }

        protected virtual Task TransitionStoppedReady()
        {
            return Task.CompletedTask;
        }

        protected virtual Task TransitionReadyPlaying()
        {
            return Task.CompletedTask;
        }

        protected virtual Task TransitionPlayingReady()
        {
            return Task.CompletedTask;
        }

        protected virtual Task TransitionReadyStopped()
        {
            return Task.CompletedTask;
        }

        public virtual IEnumerable<PropertyValue> GetPropertyValues()
        {
            return this.GetPropertyBindings().Select(pValue => pValue.GetValue());
        }
        public abstract IPipeSrcPad? GetSrcPad(string fromPad);
        public abstract IPipeSinkPad? GetSinkPad(string toPad);


        protected abstract IEnumerable<IPropertyBinding> GetPropertyBindings();

        public virtual bool SetPropertyValue(PropertyValue propvalue)
        {
            var setter = this.GetPropertyBindings().FirstOrDefault(setter => setter.TrySetValue(propvalue));

            return setter != null;
        }
    }
}
