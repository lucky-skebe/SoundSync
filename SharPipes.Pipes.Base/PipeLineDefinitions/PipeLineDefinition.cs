using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.PipeLineDefinitions
{
    public class PipeLineDefinition
    {

        public PipeLineDefinition() : this(new List<ElementDefinition>(), new List<LinkDefinition>())
        {
        }

        public PipeLineDefinition(IList<ElementDefinition> elements, IList<LinkDefinition> links)
        {
            this.Elements = elements;
            this.Links = links;
        }

        public IList<ElementDefinition> Elements { get; }
        public IList<LinkDefinition> Links { get; }
    }
}
