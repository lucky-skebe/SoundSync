using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.Events
{
    public class ElementAddedEventArgs
    {
        public IPipeElement Element { get; }

        public ElementAddedEventArgs(IPipeElement element)
        {
            Element = element;
        }
    }
}
