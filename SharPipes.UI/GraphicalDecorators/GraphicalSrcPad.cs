using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.UI.GraphicalDecorators
{
    public class GraphicalSrcPad : Graphical<IPipeSrcPad>
    {
        private int padNr;

        public GraphicalElement Parent { get; }
        public override double X => Parent.X + 95;

        public override double Y => Parent.Y + padNr * 15 + 10;

        public override int ZIndex => (int)ZLayer.Pads;
        public GraphicalSrcPad(IPipeSrcPad element, GraphicalElement parent, int padNr = 0) : base(element)
        {
            this.padNr = padNr;
            this.Parent = parent;
            this.Parent.PropertyChanged += this.Parent_PropertyChanged;
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X")
            {
                this.OnPropertyChanged("X");
            }
            else if (e.PropertyName == "Y")
            {
                this.OnPropertyChanged("Y");
            }
        }
    }
}
