using System.Collections.Generic;
using System.Windows;
using SharPipes.Pipes.Base.PipeLineDefinitions;

namespace SharPipes.UI.GraphicalDecorators
{
    internal class GraphicalElementDefinition
    {
        public ElementDefinition Element { get; }
        public double X { get; }
        public double Y { get; }

        public GraphicalElementDefinition(ElementDefinition element, double x, double y)
        {
            Element = element;
            X = x;
            Y = y;
        }
    }
}