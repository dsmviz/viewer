using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using System.Diagnostics;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementCreateAction(
        IElementModelEditing elementModelEditing,
        string name,
        string type,
        IElement parent,
        int index)
        : IAction
    {
        private IElement? _element;

        public const ActionType RegisteredType = ActionType.ElementCreate;

        public ActionType Type => RegisteredType;
        public string Title => "Create element";
        public string Description => $"name={name} type={type} parent={parent.Fullname}";

        public object? Do()
        {
            _element = elementModelEditing.AddElement(name, type, parent.Id, index, null);
            Debug.Assert(_element != null);

            elementModelEditing.AssignElementOrder();

            return _element;
        }

        public void Undo()
        {
            if (_element != null)
            {
                elementModelEditing.RemoveElement(_element.Id);
                elementModelEditing.AssignElementOrder();
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
