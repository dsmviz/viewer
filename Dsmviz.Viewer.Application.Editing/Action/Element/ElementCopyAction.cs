using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Application.Editing.Action.Base;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementCopyAction(
        IElement element,
        IActionContext actionContext)
        : IAction
    {
        public const ActionType RegisteredType = ActionType.ElementCopy;

        public ActionType Type => RegisteredType;
        public string Title => "Copy element";
        public string Description => $"element={element.Fullname}";

        public object? Do()
        {
            actionContext.AddElementToClipboard(element);

            return null;
        }

        public void Undo()
        {
            actionContext.RemoveElementFromClipboard(element);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
