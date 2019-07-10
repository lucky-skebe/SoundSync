using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public abstract class ValueParameterInteraction<TValue> : IInteraction
    {
        private readonly Func<TValue> getValue;
        private readonly Action<TValue> setValue;

        public ValueParameterInteraction(string name, Func<TValue> GetValue, Action<TValue> SetValue)
        {
            this.Name = name;
            getValue = GetValue;
            setValue = SetValue;
        }

        public string Name { get; }

        public TValue Value {
            get => getValue();
            set => setValue(value);
        }
    }
}
