using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public class PipeSrcPad<TValue> : IPipeSrcPad
    {
        public PipeSrcPad(IPipeSrc Parent, string name)
        {
            this.Parent = Parent;
            Name = name;
        }



        public PipeEdge<TValue>? Edge { get; internal set; }
        public IPipeSrc Parent {
            get;
            protected set;
        }
        public string Name { get; }

        public void Push(TValue value)
        {
            if(Edge != null)
            {
                Edge.Push(value);
            }
        }

        public void Unlink()
        {
            this.Edge?.Unlink();
            this.Edge = null;
        }
    }
}
