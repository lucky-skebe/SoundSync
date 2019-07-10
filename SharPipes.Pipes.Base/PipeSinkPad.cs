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

        public PipeSinkPad(IPipeSink Parent, Action<TValue> elemFunc)
        {
            this.ElemFunc = elemFunc;
            this.Parent = Parent;
        }

        public IPipeSink Parent
        {
            get;
            protected set;
        }

        public PipeEdge<TValue>? Edge { get; internal set; }

        public void Push(TValue value)
        {
            this.ElemFunc(value);
        }

        public void Unlink()
        {
            this.Edge?.Unlink();
            this.Edge = null;
        }
    }
}
