using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class StringParameterInteraction : ValueParameterInteraction<string>
    {
        public StringParameterInteraction(string name, Func<string> GetValue, Action<string> SetValue) : base(name, GetValue, SetValue)
        {
        }
    }
}
