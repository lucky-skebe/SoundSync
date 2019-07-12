using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace SharPipes.UI.GraphicalDecorators
{
    public class GraphicalSinkPad : Graphical<IPipeSinkPad>
    {
        public GraphicalElement Parent { get; }
        public Vector Offset{ get; set; }
        public override double X => Parent.X - 5;
        public override double Y => Parent.Y + padNr * 15 + 10;

        public override int ZIndex => (int)ZLayer.Pads;

        private int padNr;

        public GraphicalSinkPad(IPipeSinkPad element, GraphicalElement parent, int padNr = 0) : base(element)
        {
            this.padNr = padNr;
            this.Parent = parent;

            this.Parent.PropertyChanged += Parent_PropertyChanged;
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ( e.PropertyName == "X")
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
