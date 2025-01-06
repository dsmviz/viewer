using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Matrix
{
    public class CycleEventArgs(IElement consumer, IElement provider, Cycle oldValue, Cycle newValue)
        : EventArgs
    {
        public IElement Consumer { get; } = consumer;
        public IElement Provider { get; } = provider;
        public Cycle OldValue { get; } = oldValue;
        public Cycle NewValue { get; } = newValue;
    }
}
