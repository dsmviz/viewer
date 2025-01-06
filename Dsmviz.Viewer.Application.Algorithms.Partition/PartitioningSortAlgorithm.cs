using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Algorithms.Common;

namespace Dsmviz.Viewer.Application.Algorithms.Partition
{
    public class PartitionSortAlgorithm : ISortAlgorithm
    {
        public const string AlgorithmName = "Partition";

        public ISortResult Sort(IElement element, IElementWeightMatrix dependencyWeights)
        {

            SortResult vector = new SortResult(element.Children.Count);
            if (element.Children.Count > 1)
            {
                PartitioningCalculation algorithm = new PartitioningCalculation(dependencyWeights);

                vector = algorithm.Partition();
            }

            return vector;
        }

        public string Name => AlgorithmName;
    }
}
