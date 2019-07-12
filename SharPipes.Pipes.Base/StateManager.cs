using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharPipes.Pipes.Base
{
    static class StateManager
    {
        public static IList<State> GetTransitions(State from, State to)
        {
            return (from, to) switch
            {
                (State.Stopped, State.Playing) => new List<State> { State.Ready, State.Playing },
                (State.Playing, State.Stopped) => new List<State> { State.Ready, State.Stopped },
                _ when from == to => new List<State> {  },
                (_, State newState) => new List<State> { newState },
            };
        }
    }
}
