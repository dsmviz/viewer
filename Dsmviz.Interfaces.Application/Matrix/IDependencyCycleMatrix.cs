using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Matrix
{
    public interface IDependencyCycleMatrix
    {
        event EventHandler<CycleEventArgs>? CycleChanged;

        Cycle GetCycle(IElement consumer, IElement provider);
    }
}
