using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementSortAction(
        IElementModelEditing elementModelEditing,
        IElement element,
        IElementWeightMatrix weightsMatrix,
        ISortAlgorithm sortAlgorithm)
        : IAction
    {
        private ISortResult? _sortResult;

        public const ActionType RegisteredType = ActionType.ElementSort;

        public ActionType Type => RegisteredType;
        public string Title => "Partition element";
        public string Description => $"element={element.Fullname} algorithm={sortAlgorithm.Name}";

        public object? Do()
        {
            _sortResult = sortAlgorithm.Sort(element, weightsMatrix);
            elementModelEditing.ReorderChildren(element, _sortResult.SortedIndexValues);

            elementModelEditing.AssignElementOrder();

            return null;
        }

        public void Undo()
        {
            if (_sortResult != null)
            {
                _sortResult.InvertOrder();
                elementModelEditing.ReorderChildren(element, _sortResult.SortedIndexValues);

                elementModelEditing.AssignElementOrder();
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
