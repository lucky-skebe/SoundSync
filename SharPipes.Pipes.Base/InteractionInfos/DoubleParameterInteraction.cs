using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class DoubleParameterInteraction : ValueParameterInteraction<double>
    {
        public DoubleParameterInteraction(string name, Func<double> GetValue, Action<double> SetValue) : base(name, GetValue, SetValue)
        {
        }
    }
}
