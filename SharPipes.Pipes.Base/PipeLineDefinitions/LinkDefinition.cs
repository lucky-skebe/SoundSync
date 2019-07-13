using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.PipeLineDefinitions
{
    public class LinkDefinition
    {
        public LinkDefinition(string fromElement, string fromPad, string toElement, string toPad)
        {
            FromElement = fromElement;
            FromPad = fromPad;
            ToElement = toElement;
            ToPad = toPad;
        }

        public string FromElement { get; }
        public string FromPad { get; }
        public string ToElement { get; }
        public string ToPad { get; }
    }
}
