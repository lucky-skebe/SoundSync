using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace SharPipes.UI.GraphicalDecorators
{
    public class GraphicalElement : Graphical<IPipeElement>, IEquatable<GraphicalElement>
    {
        private Point position;
        private readonly GraphicalPipeline pipeline;

        public override double X => position.X;
        public override double Y => position.Y;

        public string Name => Element.Name;


        public void MoveTo(Point position)
        {
            this.position = position;
            this.OnPropertyChanged("X");
            this.OnPropertyChanged("Y");
        }

        public GraphicalElement(IPipeElement element, Point position, GraphicalPipeline pipeline) : base(element)
        {
            this.position = position;
            this.pipeline = pipeline;
        }

        internal void Delete()
        {
            this.pipeline.Remove(this);
        }

        public bool Equals(GraphicalElement other)
        {
            return this.Element.Equals(other.Element);
        }
    }
}
