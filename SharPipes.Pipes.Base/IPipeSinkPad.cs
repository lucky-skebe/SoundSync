using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPipeSinkPad
    {
        IPipeSink Parent { get; }
    }
}
