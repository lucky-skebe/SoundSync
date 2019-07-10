using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.InteractionInfos
{
    public interface ISelectable<T>
    {
        bool Selected { get; set; }

        T Value { get; }
    }
}
