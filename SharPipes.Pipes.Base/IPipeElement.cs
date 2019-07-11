﻿using SharPipes.Pipes.Base.InteractionInfos;
using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public interface IPipeElement
    {
        String Name { get; }

        Task Start();
        Task Stop();

        GraphState Check();

        string TypeName { get; }

        IEnumerable<IPipeElement> GetPrevNodes();

        IEnumerable<IInteraction> Interactions { get; }

        IEnumerable<IPipeSinkPad> GetSinkPads();

        IEnumerable<IPipeSrcPad> GetSrcPads();

        IEnumerable<PropertyValue> GetPropertyValues();
        bool SetPropertyValue(PropertyValue propvalue);
        IPipeSrcPad? GetSrcPad(string fromPad);
        IPipeSinkPad? GetSinkPad(string toPad);
    }
}
