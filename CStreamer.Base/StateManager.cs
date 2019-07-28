// -----------------------------------------------------------------------
// <copyright file="StateManager.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Base
{
    using System.Collections.Generic;

    public static class StateManager
    {
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
