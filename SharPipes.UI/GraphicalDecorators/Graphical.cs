using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharPipes.UI.GraphicalDecorators
{
    public abstract class Graphical<TPipe> : IGraphical, INotifyPropertyChanged
    {
        public virtual double X { get; }
        public virtual double Y { get; }

        public TPipe Element { get; }

        public Guid Id { get; }

        public Graphical(TPipe element)
        {
            this.Id = Guid.NewGuid();
            this.Element = element;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        public bool Equals(IGraphical other)
        {
            return this.Id == other.Id;
        }
    }
}
