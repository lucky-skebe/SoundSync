// -----------------------------------------------------------------------
// <copyright file="PipeEdge.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    internal class PipeEdge<TValue> : IPipeLink
    {
        internal PipeEdge(PipeSrcPad<TValue> src, PipeSinkPad<TValue> sink)
        {
            this.Sink = sink;
            this.Src = src;
        }

        public PipeSrcPad<TValue> Src { get; }

        public PipeSinkPad<TValue> Sink { get; }

        IPipeSrcPad IPipeLink.Src => this.Src;

        IPipeSinkPad IPipeLink.Sink => this.Sink;

        public void Unlink()
        {
            this.Src.Edge = null;
            this.Sink.Edge = null;
        }

        internal void Push(TValue value)
        {
            this.Sink.Push(value);
        }
    }
}
