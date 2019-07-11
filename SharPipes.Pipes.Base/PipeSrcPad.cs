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
            this.Name = name;
        }

        internal PipeEdge<TValue>? Edge { get; set; }
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

        public bool Equals(IPipeSrcPad other) => Parent.Equals(other.Parent) && Name.Equals(other.Name);

        public bool IsLinked => this.Edge != null;

        public PipeSinkPad<TValue>? Peer => this.Edge?.To;

        IPipeSinkPad? IPipeSrcPad.Peer => this.Peer;

        public override int GetHashCode()
        {
            return (this.Parent, this.Name).GetHashCode();
        }
    }
}
