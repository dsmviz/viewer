using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementChangeTypeAction : IAction
    {
        private readonly IElementModelEditing _elementModelEditing;
        private readonly IElement _element;
        private readonly string _oldType;
        private readonly string _newType;

        public const ActionType RegisteredType = ActionType.ElementChangeType;
        public ElementChangeTypeAction(IElementModelEditing elementModelEditing, IElement element, string type)
        {
            _elementModelEditing = elementModelEditing;
            _element = element;
            _oldType = _element.Type;
            _newType = type;
        }

        public ActionType Type => RegisteredType;
        public string Title => "Change element type";
        public string Description => $"element={_element.Fullname} type={_oldType}->{_newType}";

        public object? Do()
        {
            _elementModelEditing.ChangeElementType(_element, _newType);
            return null;
        }

        public void Undo()
        {
            _elementModelEditing.ChangeElementType(_element, _oldType);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
