using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPropertySetter
    {
        bool TrySetValue(PropertyValue propvalue);
    }
}
