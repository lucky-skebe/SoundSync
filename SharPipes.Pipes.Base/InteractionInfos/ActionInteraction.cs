using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class ActionInteraction : IInteraction
    {

        public ActionInteraction(Action action, string name)
        {
            this.Action = action;
            Name = name;
        }

        public Action Action { get; }
        public string Name { get; }
    }
}
