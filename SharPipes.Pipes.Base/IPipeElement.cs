using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharPipes.Pipes.Base
{
    public interface IPipeElement
    {
        public Guid Id { get; }
        Task Start();
        Task Stop();

        GraphState Check();

        IEnumerable<IPipeElement> GetPrevNodes();

        IEnumerable<ParameterInfo> DescribeParameters();
    }
}
