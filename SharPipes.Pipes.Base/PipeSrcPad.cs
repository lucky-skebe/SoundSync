using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public class PipeSrcPad<TValue> : IPipeSrcPad
    {
        public PipeSrcPad(IPipeSrc Parent)
        {
            this.Parent = Parent;
        }

        public PipeEdge<TValue>? Edge { get; internal set; }
        public IPipeSrc Parent {
            get;
            protected set;
        }

        public void Push(TValue value)
        {
            if(Edge != null)
            {
                Edge.Push(value);
            }
        }
    }
}
