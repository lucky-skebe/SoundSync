// -----------------------------------------------------------------------
// <copyright file="StateManager.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base
{
    using System.Collections.Generic;

    /// <summary>
    /// A helperclass that returns all <see cref="State">States</see> a State treansition should go throu.
    /// </summary>
    public static class StateManager
    {
        /// <summary>
        /// Gets a list of all states that should be gone throu to reach the given state.
        /// </summary>
        /// <param name="from">The original State.</param>
        /// <param name="to">The desired State.</param>
        /// <returns>A list of all states that should be gone throu.</returns>
        public static IList<State> GetTransitions(State from, State to)
        {
            return (from, to) switch
            {
                (State.Stopped, State.Playing) => new List<State> { State.Ready, State.Playing },
                (State.Playing, State.Stopped) => new List<State> { State.Ready, State.Stopped },
                _ when from == to => new List<State> { },
                (_, State newState) => new List<State> { newState },
            };
        }
    }
}
