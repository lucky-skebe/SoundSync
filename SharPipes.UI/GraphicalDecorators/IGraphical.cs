using System;
using System.Collections.Generic;
using System.Text;

namespace SharPipes.UI.GraphicalDecorators
{
    public interface IGraphical : IEquatable<IGraphical>
    {
        Guid Id { get; }
        double X { get; }
        double Y { get; }
    }
}
