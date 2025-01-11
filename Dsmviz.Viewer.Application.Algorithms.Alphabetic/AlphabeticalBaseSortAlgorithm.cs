using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Application.Algorithms.Common;

namespace Dsmviz.Viewer.Application.Algorithms.Alphabetic
{
    public abstract class AlphabeticalBaseSortAlgorithm
    {
        public ISortResult Sort(IElement element, IElementWeightMatrix dependencyWeights)
        {
            SortResult vector = new SortResult(element.Children.Count);

            if (element.Children.Count > 1)
            {
                List<int> newOrder = Reorder(element);

                for (int i = 0; i < vector.GetNumberOfElements(); i++)
                {
                    int id = element.Children[i].Id;
                    vector.SetIndex(newOrder.IndexOf(id), i);
                }
            }

            return vector;
        }

        protected abstract List<int> Reorder(IElement element);
    }
}
