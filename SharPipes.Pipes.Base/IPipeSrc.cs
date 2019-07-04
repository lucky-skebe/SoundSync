using System;

namespace SharPipes.Pipes.Base
{
    public interface IPipeSrc: IPipeElement
    {
        PipeSrcPad<TValue>? GetSrc<TValue>(string name);
    }
}