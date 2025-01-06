using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Matrix
{
    public interface IDerivedWeight
    {
        IElement Consumer { get; }
        IElement Provider { get; }
        int Weight { get; }
    }
}
