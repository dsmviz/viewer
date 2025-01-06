using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Matrix
{
    public interface IDependencyWeightMatrix
    {
        event EventHandler<WeightEventArgs>? DerivedWeightChanged;

        int GetDirectDependencyWeight(IElement consumer, IElement provider);

        /// <summary>
        /// Get the calculated derived dependency weight between the specified consumer and provider based of the weight of the relation
        /// between the consumer and provider including their children.
        /// </summary>
        int GetDerivedDependencyWeight(IElement consumer, IElement provider);
    }
}
