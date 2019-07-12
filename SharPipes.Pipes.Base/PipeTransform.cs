﻿using SharPipes.Pipes.Base.InteractionInfos;
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
        public State CurrentState { get; private set; }

        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        public abstract PipeSinkPad<TValue>? GetSink<TValue>(string name);

        public abstract IEnumerable<IPipeSinkPad> GetSinkPads();

        public abstract PipeSrcPad<TValue>? GetSrc<TValue>(string name);

        public abstract IEnumerable<IPipeSrcPad> GetSrcPads();

        public async Task GoToState(State newState)
        {
            var transitions = StateManager.GetTransitions(CurrentState, newState);

            foreach(var transition in transitions)
            {
                await ((this.CurrentState, transition) switch
                {
                    (State.Stopped, State.Ready) => this.TransitionStoppedReady(),
                    (State.Ready, State.Playing) => this.TransitionReadyPlaying(),
                    (State.Playing, State.Ready) => this.TransitionReadyStopped(),
                    (State.Ready, State.Stopped) => this.TransitionReadyStopped(),
                    _ => Task.CompletedTask,
                });
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
