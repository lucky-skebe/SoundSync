using System;

namespace SharPipes.Pipes.Base
{
    public class PipeEdge<TValue> : IPipeEdge
    {
        internal PipeEdge(PipeSrcPad<TValue> From, PipeSinkPad<TValue> To)
        {
            this.To = To;
            this.From = From;
        }

        public PipeSrcPad<TValue> From { get; }
        public PipeSinkPad<TValue> To { get; }

        IPipeSrcPad IPipeEdge.From => this.From;

        IPipeSinkPad IPipeEdge.To => this.To;

        internal void Push(TValue value)
        {
            To.Push(value);
        }
    }
}
