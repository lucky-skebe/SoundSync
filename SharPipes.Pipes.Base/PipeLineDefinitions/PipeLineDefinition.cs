using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.PipeLineDefinitions
{
    public class PipeLineDefinition
    {

        public PipeLineDefinition()
        {
            this.Elements = new List<ElementDefinition>();
            this.Links = new List<LinkDefinition>();
        }

        public List<ElementDefinition> Elements { get; }
        public List<LinkDefinition> Links { get; }
    }
}
