﻿// -----------------------------------------------------------------------
// <copyright file="Element.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    using Optional;
    using Optional.Collections;
    using SharPipes.Pipes.Base.Attributes;
    using SharPipes.Pipes.Base.PipeLineDefinitions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Baseclass for all element.
    /// </summary>
    public abstract class Element : IElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeElement"/> class.
        /// </summary>
        /// <param name="name">the name ot the element.</param>
        protected Element(string? name = null)
        {
            this.Name = name ?? $"{GetName(this.GetType())}-{Guid.NewGuid()}";
            this.CurrentState = State.Stopped;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public State CurrentState { get; private set; }

        /// <summary>
        /// Gets the name for a given Type.
        ///
        /// These names can either be registered using the <see cref="ElementNameAttribute"/> or will be generated using the Classname.
        /// Classnames ending in Src, Sink, or Element will get these parts removed.
        /// </summary>
        /// <param name="type">the type to resolve the name of.</param>
        /// <returns>The factoryType name of the given type.</returns>
        /// <exception cref="ArgumentNullException">if type is null.</exception>
        public static string GetName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            ElementNameAttribute? attribute;
            if ((attribute = type.GetCustomAttribute<ElementNameAttribute>()) != null)
            {
                return attribute.Name;
            }
            else
            {
                var typeName = type.Name;

                if (TrimEnd(typeName, "element", out string trimmed))
                {
                    return trimmed;
                }
                else if (TrimEnd(typeName, "src", out trimmed))
                {
                    return trimmed;
                }
                else if (TrimEnd(typeName, "sink", out trimmed))
                {
                    return trimmed;
                }

                return type.Name;
            }
        }

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

        private static bool TrimEnd(string from, string end, out string trimmed)
        {
            trimmed = from;
            if (from.EndsWith(end, StringComparison.OrdinalIgnoreCase))
            {
                var index = from.LastIndexOf(end, StringComparison.OrdinalIgnoreCase);
                trimmed = from.Substring(0, index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a list of all Property bindings that should be serialized/deserialized.
        /// </summary>
        /// <returns>List of all the PropertyBindings of hte element.</returns>
        public abstract IEnumerable<IPropertyBinding> GetPropertyBindings();
        public abstract IEnumerable<IPad> GetPads();
    }
}
