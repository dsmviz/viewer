

using System.ComponentModel;
using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Utils;
using Dsmviz.Viewer.ViewModel.Common;
using System.Windows.Input;
using Dsmviz.ViewModel.Interfaces.Editing.Element;

namespace Dsmviz.Viewer.ViewModel.Editing.Element
{
    public class ElementEditViewModel : ViewModelBase, IElementEditViewModel
    {
        private readonly IElementEditing _elementEditing;
        private readonly IElement? _parentElement;
        private readonly IElement? _selectedElement;
        private readonly int? _addAtIndex;
        private string _name;
        private string _help;
        private string _selectedElementType;

        private static string _lastSelectedElementType = "";

        public ICommand? AcceptChangeCommand { get; }

        public ElementEditViewModel(ElementEditViewModelType viewModelType, IElementEditing elementEditing, IElement selectedElement)
        {
            _elementEditing = elementEditing;

            ElementTypes = [.. _elementEditing.GetElementTypes()];

            switch (viewModelType)
            {
                case ElementEditViewModelType.Modify:
                    _parentElement = selectedElement.Parent;
                    _selectedElement = selectedElement;
                    _addAtIndex = 0;

                    Title = "Modify element";

                    _name = _selectedElement.Name;
                    _selectedElementType = _selectedElement.Type;
                    _help = string.Empty;

                    AcceptChangeCommand = RegisterCommand(AcceptModifyExecute, AcceptCanExecute);
                    break;
                case ElementEditViewModelType.AddChild:
                    _parentElement = selectedElement;
                    _selectedElement = null;
                    _addAtIndex = _parentElement.Children.Count; // Insert at end

                    Title = "Add element";

                    _name = string.Empty;
                    _selectedElementType = _lastSelectedElementType;
                    _help = string.Empty;

                    AcceptChangeCommand = RegisterCommand(AcceptAddExecute, AcceptCanExecute);
                    break;
                case ElementEditViewModelType.AddSiblingAbove:
                    _parentElement = selectedElement.Parent;
                    _selectedElement = selectedElement;
                    _addAtIndex = _parentElement?.IndexOfChild(_selectedElement);

                    Title = "Add element";

                    _name = string.Empty;
                    _selectedElementType = _selectedElement.Type;
                    _help = string.Empty;

                    AcceptChangeCommand = RegisterCommand(AcceptAddExecute, AcceptCanExecute);
                    break;
                case ElementEditViewModelType.AddSiblingBelow:
                    _parentElement = selectedElement.Parent;
                    _selectedElement = selectedElement;
                    _addAtIndex = _parentElement?.IndexOfChild(_selectedElement) + 1;

                    Title = "Add element";

                    _name = string.Empty;
                    _selectedElementType = _selectedElement.Type;
                    _help = string.Empty;

                    AcceptChangeCommand = RegisterCommand(AcceptAddExecute, AcceptCanExecute);
                    break;
                default:
                    Title = string.Empty;

                    _name = string.Empty;
                    _selectedElementType = string.Empty;
                    _help = string.Empty;

                    break;
            }
        }

        public string Title { get; }

        public string Help
        {
            get => _help;
            private set { _help = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public List<string> ElementTypes { get; }

        public string SelectedElementType
        {
            get => _selectedElementType;
            set { _selectedElementType = value; _lastSelectedElementType = value; OnPropertyChanged(); }
        }

        private void AcceptAddExecute(object? parameter)
        {
            if (_addAtIndex.HasValue && _parentElement != null)
            {
                _elementEditing.CreateElement(Name, SelectedElementType, _parentElement, _addAtIndex.Value);
            }
        }

        private void AcceptModifyExecute(object? parameter)
        {
            if (_selectedElement != null)
            {
                if (_selectedElement.Name != Name)
                {
                    _elementEditing.ChangeElementName(_selectedElement, Name);
                }

                if (_selectedElement.Type != SelectedElementType)
                {
                    _elementEditing.ChangeElementType(_selectedElement, SelectedElementType);
                }
            }
        }

        private bool AcceptCanExecute(object? parameter)
        {
            if (_parentElement != null)
            {
                ElementName elementName = new ElementName(_parentElement.Fullname);
                elementName.AddNamePart(Name);
                IElement? existingElement = _elementEditing.GetElementByFullname(elementName.FullName);

                if (Name.Length == 0)
                {
                    Help = "Name can not be empty";
                    return false;
                }
                else if (Name.Contains("."))
                {
                    Help = "Name can not be contain dot character";
                    return false;
                }
                else if ((existingElement != _selectedElement) && (existingElement != null))
                {
                    Help = "Name can not be an existing name";
                    return false;
                }
                else
                {
                    Help = "";
                    return true;
                }
            }
            else
            {
                Help = "Parent not found";
                return false;
            }
        }
    }
}
