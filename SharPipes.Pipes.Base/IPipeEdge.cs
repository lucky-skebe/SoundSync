using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPipeEdge
    {
        public IPipeSrcPad From { get; }
        public IPipeSinkPad To { get; }
    }
}
