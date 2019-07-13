using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.PipeLineDefinitions
{
    public class ElementDefinition
    {
        public ElementDefinition(string typeFactory, string name, IList<PropertyValue> properties)
        {
            TypeFactory = typeFactory;
            Name = name;
            Properties = properties;
        }

        public string TypeFactory { get; }
        public string Name { get; }
        public IList<PropertyValue> Properties { get; }
    }
}
