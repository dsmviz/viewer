using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Application.Algorithms.Alphabetic
{
    public class AlphabeticalDescendingSortAlgorithm : AlphabeticalBaseSortAlgorithm, ISortAlgorithm
    {
        public const string AlgorithmName = "AlphabeticalDescending";

        public string Name => AlgorithmName;

        protected override List<int> Reorder(IElement element)
        {
            return element.Children.OrderByDescending(x => x.Name).Select(x => x.Id).ToList();
        }
    }
}
