using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Base;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementCutAction(
        IElementModelEditing elementModelEditing,
        IElement element,
        IActionContext actionContext)
        : IAction
    {
        public const ActionType RegisteredType = ActionType.ElementCut;

        public ActionType Type => RegisteredType;
        public string Title => "Cut element";
        public string Description => $"element={element.Fullname}";

        public object? Do()
        {
            actionContext.AddElementToClipboard(element);

            elementModelEditing.RemoveElement(element.Id);
            elementModelEditing.AssignElementOrder();
            return null;
        }

        public void Undo()
        {
            actionContext.RemoveElementFromClipboard(element);

            elementModelEditing.RestoreElement(element.Id);
            elementModelEditing.AssignElementOrder();
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
