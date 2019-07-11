using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public class PipeSinkPad<TValue> : IPipeSinkPad
    {
        protected Action<TValue> ElemFunc{
            get;
            set;
          }

        public PipeSinkPad(IPipeSink Parent, string name, Action<TValue> elemFunc)
        {
            this.Name = name;
            this.ElemFunc = elemFunc;
            this.Parent = Parent;
        }

        public IPipeSink Parent
        {
            get;
            protected set;
        }

        internal PipeEdge<TValue>? Edge { get; set; }

        public void Push(TValue value)
        {
            this.ElemFunc(value);
        }

        public void Unlink()
        {
            this.Edge?.Unlink();
            this.Edge = null;
        }

        public bool Equals(IPipeSinkPad other) => Parent.Equals(other.Parent) && Name.Equals(other.Name);

        public bool IsLinked => this.Edge != null;


        public PipeSrcPad<TValue>? Peer => this.Edge?.From;

        IPipeSrcPad? IPipeSinkPad.Peer => this.Peer;

        public string Name { get; }

        public override int GetHashCode()
        {
            return (this.Parent, this.Name).GetHashCode();
        }
    }
}
