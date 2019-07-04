using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.Pipes.Base
{
    public enum ParameterType
    {
        Float
    }

    public class ParameterInfo
    {
        public ParameterInfo(string name, ParameterType parameterType, string binding)
        {
            this.Binding = binding;
            this.ParameterType = parameterType;
            this.Name = name;
        }

        public string Name { get; }

        public ParameterType ParameterType { get; }

        public string Binding { get; }
    }
}
