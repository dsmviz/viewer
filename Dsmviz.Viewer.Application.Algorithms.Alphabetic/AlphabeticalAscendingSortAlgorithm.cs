using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Application.Algorithms.Alphabetic
{
    public class AlphabeticalAscendingSortAlgorithm : AlphabeticalBaseSortAlgorithm, ISortAlgorithm
    {
        public const string AlgorithmName = "AlphabeticalAscending";

        public string Name => AlgorithmName;

        protected override List<int> Reorder(IElement element)
        {
            return element.Children.OrderBy(x => x.Name).Select(x => x.Id).ToList();
        }
    }
}
