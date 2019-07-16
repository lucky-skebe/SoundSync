// -----------------------------------------------------------------------
// <copyright file="Link.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.Pipes.Base
{
    internal class Link<TValue> : ILink<TValue>
    {
        internal Link(SrcPad<TValue> src, SinkPad<TValue> sink)
        {
            this.Sink = sink;
            this.Src = src;
        }

        public SrcPad<TValue> Src { get; }

        public SinkPad<TValue> Sink { get; }

        ISrcPad ILink.Src => this.Src;

        ISrcPad<TValue> ILink<TValue>.Src => this.Src;

        ISinkPad ILink.Sink => this.Sink;

        ISinkPad<TValue> ILink<TValue>.Sink => this.Sink;

        public void Push(TValue value)
        {
            this.Sink.Push(value);
        }

        public void Unlink()
        {
            this.Src.Unlink();
            this.Sink.Unlink();
        }
    }
}
