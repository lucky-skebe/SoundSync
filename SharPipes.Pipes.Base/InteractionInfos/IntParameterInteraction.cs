using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class IntParameterInteraction : ValueParameterInteraction<int>
    {
        public IntParameterInteraction(string name, Func<int> GetValue, Action<int> SetValue) : base(name, GetValue, SetValue)
        {
        }
    }
}
