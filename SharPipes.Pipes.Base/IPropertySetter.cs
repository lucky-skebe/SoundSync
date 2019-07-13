using SharPipes.Pipes.Base.PipeLineDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public interface IPropertyBinding
    {
        bool TrySetValue(PropertyValue propvalue);

        PropertyValue GetValue();
    }
}
