using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class FloatParameterInteraction : ValueParameterInteraction<float>
    {
        public FloatParameterInteraction(string name, Func<float> GetValue, Action<float> SetValue) : base(name, GetValue, SetValue)
        {
        }
    }
}
