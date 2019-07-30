// -----------------------------------------------------------------------
// <copyright file="Element.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Base
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CStreamer.Base;
    using CStreamer.Plugins.Interfaces;

    /// <summary>
    /// Baseclass for all element.
    /// </summary>
    public abstract class Element : IElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="name">the name ot the element.</param>
        protected Element(string? name)
        {
            this.Name = name ?? $"{this.GetElementName()}-{Guid.NewGuid()}";
            this.CurrentState = State.Stopped;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public State CurrentState { get; private set; }

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

        /// <summary>
        /// Gets a list of all Property bindings that should be serialized/deserialized.
        /// </summary>
        /// <returns>List of all the PropertyBindings of hte element.</returns>
        public abstract IEnumerable<IPropertyBinding> GetPropertyBindings();

        /// <inheritdoc/>
        public abstract IEnumerable<IPad> GetPads();

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
    }
}
