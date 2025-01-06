using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementChangeParentAction : IAction
    {
        private readonly IElementModelEditing _elementModelEditing;
        private readonly IElement _element;
        private readonly IElement? _oldParent;
        private readonly int? _oldIndex;
        private readonly string _oldName;
        private readonly IElement _newParent;
        private readonly int _newIndex;
        private string _newName;

        public const ActionType RegisteredType = ActionType.ElementChangeParent;

        public ElementChangeParentAction(IElementModelEditing modelEditing, IElement element, IElement newParent, int index)
        {
            _elementModelEditing = modelEditing;
            _element = element;

            _oldParent = element.Parent;
            _oldIndex = _oldParent?.IndexOfChild(element);
            _oldName = element.Name;

            _newParent = newParent;
            _newIndex = index;
            _newName = element.Name;
        }

        public ActionType Type => RegisteredType;
        public string Title => "Change element parent";
        public string Description => $"element={_element.Fullname} parent={_oldParent?.Fullname}->{_newParent.Fullname}";

        public object? Do()
        {
            // Rename to avoid duplicate names
            if (_newParent.ContainsChildWithName(_oldName))
            {
                _newName += " (duplicate)";
                _elementModelEditing.ChangeElementName(_element, _newName);
            }

            _elementModelEditing.ChangeElementParent(_element, _newParent, _newIndex);
            _elementModelEditing.AssignElementOrder();
            return null;
        }

        public void Undo()
        {
            if (_oldIndex.HasValue && _oldParent != null)
            {
                _elementModelEditing.ChangeElementParent(_element, _oldParent, _oldIndex.Value);
                _elementModelEditing.AssignElementOrder();

                // Restore original name
                if (_oldName != _newName)
                {
                    _elementModelEditing.ChangeElementName(_element, _oldName);
                }
            }
        }

        public bool IsValid()
        {
            return _oldParent != null &&
                   _oldIndex != null;
        }
    }
}
