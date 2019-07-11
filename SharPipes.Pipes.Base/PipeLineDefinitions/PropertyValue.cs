using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base.PipeLineDefinitions
{
    public class PropertyValue
    {
        public PropertyValue(string propertyName, string type, object value)
        {
            PropertyName = propertyName;
            Type = type;
            Value = value;
        }

        public string PropertyName { get; }
        public string Type { get; }
        public object Value { get; }
    }
}
