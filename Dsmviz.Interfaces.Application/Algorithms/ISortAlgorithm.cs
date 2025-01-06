using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Algorithms
{
    public interface ISortAlgorithm
    {
        /// <summary>
        /// Perform sort action and return sort result.
        /// </summary>
        ISortResult Sort(IElement element, IElementWeightMatrix dependencyWeights);
        /// <summary>
        /// The name of the sort algorithm.
        /// </summary>
        string Name { get; }
    }
}
