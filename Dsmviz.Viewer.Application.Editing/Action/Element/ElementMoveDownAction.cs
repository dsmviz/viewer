using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementMoveDownAction(IElementModelEditing elementModelEditing, IElement element) : IAction
    {
        public const ActionType RegisteredType = ActionType.ElementMoveDown;

        public ActionType Type => RegisteredType;
        public string Title => "Move down element";
        public string Description => $"element={element.Fullname}";

        public object? Do()
        {
            IElement? nextElement = elementModelEditing.NextSibling(element);
            if (nextElement != null)
            {
                elementModelEditing.Swap(element, nextElement);
                elementModelEditing.AssignElementOrder();
            }

            return null;
        }

        public void Undo()
        {
            IElement? previousElement = elementModelEditing.PreviousSibling(element);
            if (previousElement != null)
            {
                elementModelEditing.Swap(previousElement, element);
                elementModelEditing.AssignElementOrder();
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
