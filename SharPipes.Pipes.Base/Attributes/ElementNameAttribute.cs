using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    class ElementNameAttribute: Attribute
    {
        public ElementNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
