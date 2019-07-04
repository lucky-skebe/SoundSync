﻿using SharPipes.Pipes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Basic
{
    public class EnumerateElement : PipeTransform
    {
        public EnumerateElement()
        {
            Src = new PipeSrcPad<double>(this);
            Sink = new PipeSinkPad<IEnumerable<float>>(this, e =>
            {
                try
                {
                    foreach (var f in e)
                    {
                        Src.Push(f);
                    }
                } catch
                {

                }
            });
        }
        
        public PipeSinkPad<IEnumerable<float>> Sink
        {
            get;
            set;
        }

        public PipeSrcPad<double> Src
        {
            get;
            set;
        }

        public override PipeSinkPad<TValue>? GetSink<TValue>(string name)
        {
            return null;
        }

        public override PipeSrcPad<TValue>? GetSrc<TValue>(string name)
        {
            return null;
        }

        public override GraphState Check()
        {
            if (Sink.Edge == null)
            {
                return GraphState.INCOMPLETE;
            }
            else if (Src.Edge == null)
            {
                return GraphState.INCOMPLETE;
            }
            else
            {
                return GraphState.OK;
            }
        }

        public override IEnumerable<IPipeElement> GetPrevNodes()
        {
            if(Sink.Edge != null)
            {
                yield return Sink.Edge.From.Parent;
            }
        }
    }
}
