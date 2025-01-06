using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Base;

namespace Dsmviz.Viewer.Application.Editing.Action.Element
{
    public class ElementPasteAction : IAction
    {
        private readonly IElementModelEditing _elementModelEditing;
        private readonly IActionContext _actionContext;
        private readonly IElement? _element;
        private readonly IElement? _oldParent;
        private readonly int? _oldIndex;
        private readonly string? _oldName;
        private readonly IElement? _newParent;
        private readonly int _newIndex;
        private string? _newName;
        private readonly bool _pasteCutElement;

        public const ActionType RegisteredType = ActionType.ElementPaste;

        public ElementPasteAction(IElementModelEditing elementModelEditing, IElement newParent, int index, IActionContext actionContext)
        {
            _elementModelEditing = elementModelEditing;

            _actionContext = actionContext;
            _element = _actionContext.GetElementOnClipboard();

            if (_element != null)
            {
                _oldParent = _element.Parent;
                _oldIndex = _oldParent?.IndexOfChild(_element);
                _oldName = _element.Name;

                _newParent = newParent;
                _newIndex = index;
                _newName = _element.Name;

                _pasteCutElement = _element.IsDeleted;
            }
        }

        public ActionType Type => RegisteredType;
        public string Title => "Paste element";
        public string Description => $"element={_actionContext.GetElementOnClipboard()?.Fullname}";

        public object? Do()
        {
            IElement? element = null;

            if (_element != null)
            {
                if (_pasteCutElement)
                {
                    element = _element;
                    _elementModelEditing.RestoreElement(element.Id);
                }
                else
                {
                    element = _elementModelEditing.AddElement(_element.Name, _element.Type, null, null, null);
                }
            }

            // Rename to avoid duplicate names
            if (_newParent != null && _oldName != null && element != null)
            {
                IElement pastedElement = element;
                if (_newParent.ContainsChildWithName(_oldName))
                {
                    _newName += " (duplicate)";
                    _elementModelEditing.ChangeElementName(pastedElement, _newName);
                }

                _elementModelEditing.ChangeElementParent(pastedElement, _newParent, _newIndex);
                _elementModelEditing.AssignElementOrder();
                _actionContext.RemoveElementFromClipboard(element);
            }

            return null;
        }

        public void Undo()
        {
            if (_oldIndex.HasValue && _oldName != null && _oldParent != null && _element != null)
            {
                _elementModelEditing.RemoveElement(_element.Id);

                _elementModelEditing.ChangeElementParent(_element, _oldParent, _oldIndex.Value);
                _elementModelEditing.AssignElementOrder();

                // Restore original name
                if (_oldName != _newName)
                {
                    _elementModelEditing.ChangeElementName(_element, _oldName);
                }

                _actionContext.AddElementToClipboard(_element);
            }
        }

        public bool IsValid()
        {
            return _actionContext.IsElementOnClipboard() &&
                   _oldParent != null;
        }
    }
}
