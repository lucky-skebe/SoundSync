using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.Events
{
    public class ElementRemovedEventArgs
    {
        public IPipeElement Element { get; }

        public ElementRemovedEventArgs(IPipeElement element)
        {
            Element = element;
        }
    }
}
