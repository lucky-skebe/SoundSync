using System;

namespace SharPipes.Pipes.Base
{
    public interface IPipeSink : IPipeElement
    {
        PipeSinkPad<TValue>? GetSink<TValue>(string name);
    }
}