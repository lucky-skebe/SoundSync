using SharPipes.Pipes.Base.PipeLineDefinitions;
using System.Collections.Generic;
using System.Windows;

namespace SharPipes.UI.GraphicalDecorators
{
    internal class GraphicalPipeLineDefinition
    {
        public GraphicalPipeLineDefinition(PipeLineDefinition definition, Dictionary<string, Point> positions)
        {
            Definition = definition;
            Positions = positions;
        }

        public PipeLineDefinition Definition { get; }
        public Dictionary<string, Point> Positions { get; }

        public static implicit operator PipeLineDefinition(GraphicalPipeLineDefinition d)
        {
            return d.Definition;
        }
    }
}