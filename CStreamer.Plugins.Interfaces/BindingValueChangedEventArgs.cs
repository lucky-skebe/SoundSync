using System;
using System.Collections.Generic;
using System.Text;

namespace CStreamer.Plugins.Interfaces
{
    public class BindingValueChangedEventArgs
    {
        public BindingValueChangedEventArgs(object newValue)
        {
            this.NewValue = newValue;
        }

        public object NewValue { get; }
    }

    public class BindingValueChangedEventArgs<TValue> : BindingValueChangedEventArgs
    {
        public BindingValueChangedEventArgs(TValue newValue) : base(newValue)
        {
            this.NewValue = newValue;
        }

        public new TValue NewValue { get; }
    }
}
