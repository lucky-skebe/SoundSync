using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public abstract class PipeTransform : PipeElement, IPipeTransform
    {
        public PipeTransform(string? name = null) : base(name)
        {
        }
        

        public abstract PipeSinkPad<TValue>? GetSink<TValue>(string name);

        public abstract PipeSrcPad<TValue>? GetSrc<TValue>(string name);

        


    }
}
