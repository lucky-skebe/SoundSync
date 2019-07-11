using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPipeSinkPad
    {
        public IPipeSink Parent
        {
            get;
        }

        string Name { get; }

        void Unlink();
    }
}
