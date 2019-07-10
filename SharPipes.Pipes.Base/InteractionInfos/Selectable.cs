using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public class Selectable<T> : ISelectable<T>
    {
        private readonly Action<Selectable<T>, bool>? setSelectedCallback;

        private bool selected = false;

        public Selectable(T value, Action<Selectable<T>, bool>? setSelectedCallback = null)
        {
            Value = value;
            this.setSelectedCallback = setSelectedCallback;
        }
        
        public bool Selected
        {
            get => selected;
            set
            {
                if (selected != value)
                {
                    selected = value;
                    this.setSelectedCallback?.Invoke(this, value);
                }
            }
        }
        public T Value { get; }
    }
}
