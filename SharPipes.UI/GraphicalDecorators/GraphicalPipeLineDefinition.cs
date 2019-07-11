using SharPipes.Pipes.Base.PipeLineDefinitions;
using System.Collections.Generic;
using System.Linq;

namespace SharPipes.UI.GraphicalDecorators
{
    internal class GraphicalPipeLineDefinition
    {
        public GraphicalPipeLineDefinition(IList<LinkDefinition> links)
        {
            Elements = new List<GraphicalElementDefinition>();
            Links = links;
        }
        public IList<LinkDefinition> Links { get; }
        public IList<GraphicalElementDefinition> Elements { get; }

        public static implicit operator PipeLineDefinition(GraphicalPipeLineDefinition d)
        {
            return new PipeLineDefinition(d.Elements.Select(e => e.Element).ToList(), d.Links);
        }
    }
}