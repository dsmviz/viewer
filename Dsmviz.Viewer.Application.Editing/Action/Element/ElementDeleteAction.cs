using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementDeleteAction(IElementModelEditing elementModelEditing, IElement element) : IAction
    {
        public const ActionType RegisteredType = ActionType.ElementDelete;

        public ActionType Type => RegisteredType;
        public string Title => "Delete element";
        public string Description => $"element={element.Fullname}";

        public object? Do()
        {
            elementModelEditing.RemoveElement(element.Id);
            elementModelEditing.AssignElementOrder();
            return null;
        }

        public void Undo()
        {
            elementModelEditing.RestoreElement(element.Id);
            elementModelEditing.AssignElementOrder();
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
