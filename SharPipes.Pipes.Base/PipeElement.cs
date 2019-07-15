// -----------------------------------------------------------------------
// <copyright file="PipeElement.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SharPipes.Pipes.Base.InteractionInfos;
    using SharPipes.Pipes.Base.PipeLineDefinitions;

    /// <summary>
    /// Baseclass for all element.
    /// </summary>
    public abstract class PipeElement : IPipeElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeElement"/> class.
        /// </summary>
        /// <param name="name">the name ot the element.</param>
        protected PipeElement(string? name = null)
        {
            this.Name = name ?? $"{PipeElementFactory.GetName(this.GetType())}-{Guid.NewGuid()}";
            this.CurrentState = State.Stopped;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public State CurrentState { get; private set; }

        /// <inheritdoc/>
        public abstract string TypeName { get; }

        /// <inheritdoc/>
        public virtual IEnumerable<IInteraction> Interactions => Enumerable.Empty<IInteraction>();

        /// <inheritdoc/>
        public abstract GraphState Check();

        /// <inheritdoc/>
        public abstract IEnumerable<IPipeElement> GetPrevNodes();

        /// <inheritdoc/>
        public abstract IEnumerable<IPipeSinkPad> GetSinkPads();

        /// <inheritdoc/>
        public abstract IEnumerable<IPipeSrcPad> GetSrcPads();

        /// <inheritdoc/>
        public virtual async Task GoToState(State newState)
        {
            var transitions = StateManager.GetTransitions(this.CurrentState, newState);

            foreach (var transition in transitions)
            {
                switch (this.CurrentState, transition)
                {
                    case (State.Stopped, State.Ready):
                        await this.TransitionStoppedReady().ConfigureAwait(true);
                        break;
                    case (State.Ready, State.Playing):
                        await this.TransitionReadyPlaying().ConfigureAwait(true);
                        break;
                    case (State.Playing, State.Ready):
                        await this.TransitionPlayingReady().ConfigureAwait(true);
                        break;
                    case (State.Ready, State.Stopped):
                        await this.TransitionReadyStopped().ConfigureAwait(true);
                        break;
                }

                this.CurrentState = transition;
            }
        }

        /// <inheritdoc/>
        public virtual IEnumerable<PropertyValue> GetPropertyValues()
        {
            return this.GetPropertyBindings().Select(propertyValue => propertyValue.GetValue());
        }

        /// <inheritdoc/>
        public abstract IPipeSrcPad? GetSrcPad(string srcPadName);

        /// <inheritdoc/>
        public abstract IPipeSinkPad? GetSinkPad(string sinkPadName);

        /// <inheritdoc/>
        public virtual bool SetPropertyValue(PropertyValue propvalue)
        {
            var setter = this.GetPropertyBindings().FirstOrDefault(setter => setter.TrySetValue(propvalue));

            return setter != null;
        }

        /// <summary>
        /// Contains the logic that should run when changing from the Stopped to the Ready state.
        ///
        /// Usually initalization if external resources.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous state change.</returns>
        protected virtual Task TransitionStoppedReady()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Contains the logic that should run when changing from the Ready to the Playing state.
        ///
        /// Usually preparing to receive data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous state change.</returns>
        protected virtual Task TransitionReadyPlaying()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Contains the logic that should run when changing from the Playing to the Ready state.
        ///
        /// Usually stopping data processing.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous state change.</returns>
        protected virtual Task TransitionPlayingReady()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Contains the logic that should run when changing from the Ready to the Stopped state.
        ///
        /// Usually freeing of external resources.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous state change.</returns>
        protected virtual Task TransitionReadyStopped()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a list of all Property bindings that should be serialized/deserialized.
        /// </summary>
        /// <returns>List of all the PropertyBindings of hte element.</returns>
        protected abstract IEnumerable<IPropertyBinding> GetPropertyBindings();
    }
}
