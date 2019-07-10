using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class BoolParameterInteraction : ValueParameterInteraction<bool>
    {
        public BoolParameterInteraction(string name, Func<bool> GetValue, Action<bool> SetValue) : base(name, GetValue, SetValue)
        {
        }
    }
}
