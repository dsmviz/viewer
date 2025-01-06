using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementChangeNameAction : IAction
    {
        private readonly IElementModelEditing _elementModelEditing;
        private readonly IElement _element;
        private readonly string _old;
        private readonly string _new;

        public const ActionType RegisteredType = ActionType.ElementChangeName;

        public ElementChangeNameAction(IElementModelEditing elementModelEditing, IElement element, string name)
        {
            _elementModelEditing = elementModelEditing;
            _element = element;
            _old = _element.Name;
            _new = name;
        }

        public ActionType Type => RegisteredType;
        public string Title => "Change element name";
        public string Description => $"element={_element.Fullname} name={_old}->{_new}";

        public object? Do()
        {
            _elementModelEditing.ChangeElementName(_element, _new);
            return null;
        }

        public void Undo()
        {
            _elementModelEditing.ChangeElementName(_element, _old);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
