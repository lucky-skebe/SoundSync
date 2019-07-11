using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPipeSrcPad : IEquatable<IPipeSrcPad>
    {
        public IPipeSrc Parent
        {
            get;
        }

        string Name { get; }

        public void Unlink();

        public bool IsLinked{ get; }


        IPipeSinkPad? Peer { get; }
    }
}
