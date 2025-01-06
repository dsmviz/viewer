using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementMoveUpAction(IElementModelEditing elementModelEditing, IElement element) : IAction
    {
        public const ActionType RegisteredType = ActionType.ElementMoveUp;

        public ActionType Type => RegisteredType;
        public string Title => "Move up element";
        public string Description => $"element={element.Fullname}";

        public object? Do()
        {
            IElement? previousElement = elementModelEditing.PreviousSibling(element);
            if (previousElement != null)
            {
                elementModelEditing.Swap(element, previousElement);
                elementModelEditing.AssignElementOrder();
            }

            return null;
        }

        public void Undo()
        {
            IElement? nextElement = elementModelEditing.NextSibling(element);
            if (nextElement != null)
            {
                elementModelEditing.Swap(nextElement, element);
                elementModelEditing.AssignElementOrder();
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
