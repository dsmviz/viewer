using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Matrix
{
    public class WeightEventArgs(IElement consumer, IElement provider, int value) : EventArgs
    {
        public IElement Consumer { get; } = consumer;
        public IElement Provider { get; } = provider;
        public int Value { get; } = value;
    }
}
