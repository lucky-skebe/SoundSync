using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.Events
{
    public class ElementsLinkedEventArgs
    {
        public ElementsLinkedEventArgs(IPipeSrcPad src, IPipeSinkPad sink)
        {
            Src = src;
            Sink = sink;
        }

        public IPipeSrcPad Src { get; }
        public IPipeSinkPad Sink { get; }
    }
}
