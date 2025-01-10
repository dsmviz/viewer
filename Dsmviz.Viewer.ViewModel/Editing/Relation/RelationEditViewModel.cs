
using System.ComponentModel;
using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using System.Windows.Input;
using Dsmviz.Interfaces.ViewModel.Editing.Relation;

namespace Dsmviz.Viewer.ViewModel.Editing.Relation
{
    public class RelationEditViewModel : ViewModelBase, IRelationEditViewModel
    {
        private readonly IRelationEditing _relationEditing;
        private readonly IRelation? _selectedRelation;
        private readonly IElement? _selectedConsumer;
        private readonly IElement? _selectedProvider;
        private string _selectedRelationType;
        private int _weight;
        private string _help;

        private static string _lastSelectedRelationType = "";

        public event EventHandler<IRelation>? RelationUpdated;

        public RelationEditViewModel(RelationEditViewModelType viewModelType, IRelationEditing relationEditing, IRelationQuery relationQuery, IRelation? selectedRelation, IElement? selectedConsumer, IElement? selectedProvider)
        {
            _relationEditing = relationEditing;

            switch (viewModelType)
            {
                case RelationEditViewModelType.Modify:
                    _selectedRelation = selectedRelation;
                    _selectedConsumer = _selectedRelation?.Consumer;
                    _selectedProvider = _selectedRelation?.Provider;

                    Title = "Modify relation";
                    ConsumerName = _selectedConsumer?.Fullname ?? string.Empty;
                    ProviderName = _selectedProvider?.Fullname ?? string.Empty;

                    _selectedRelationType = _selectedRelation?.Type ?? string.Empty;
                    _weight = _selectedRelation?.Weight ?? 0;
                    _help = string.Empty;

                    AcceptChangeCommand = RegisterCommand(AcceptModifyExecute, AcceptCanExecute);
                    break;
                case RelationEditViewModelType.Add:
                    _selectedRelation = null;
                    _selectedConsumer = selectedConsumer;
                    _selectedProvider = selectedProvider;

                    Title = "Add relation";
                    ConsumerName = _selectedConsumer?.Fullname ?? string.Empty;
                    ProviderName = _selectedProvider?.Fullname ?? string.Empty;

                    _selectedRelationType = _lastSelectedRelationType;
                    _weight = 1;
                    _help = string.Empty;

                    AcceptChangeCommand = RegisterCommand(AcceptAddExecute, AcceptCanExecute);
                    break;
                default:
                    Title = string.Empty;
                    ConsumerName = string.Empty;
                    ProviderName = string.Empty;

                    _selectedRelationType = string.Empty;
                    _weight = 0;
                    _help = string.Empty;
                    break;
            }

            RelationTypes = [.. relationQuery.GetRelationTypes()];
        }

        public string Title { get; }

        public string ConsumerName { get; }

        public string ProviderName { get; }

        public List<string> RelationTypes { get; }

        public string SelectedRelationType
        {
            get => _selectedRelationType;
            set { _selectedRelationType = value; _lastSelectedRelationType = value; OnPropertyChanged(); }
        }

        public int Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(); }
        }

        public string Help
        {
            get => _help;
            private set { _help = value; OnPropertyChanged(); }
        }

        public ICommand? AcceptChangeCommand { get; }

        private void AcceptModifyExecute(object? parameter)
        {
            bool relationUpdated = false;
            if (_selectedRelation != null)
            {
                if (_selectedRelation.Type != SelectedRelationType)
                {
                    _relationEditing.ChangeRelationType(_selectedRelation, SelectedRelationType);
                    relationUpdated = true;
                }

                if (_selectedRelation.Weight != Weight)
                {
                    _relationEditing.ChangeRelationWeight(_selectedRelation, Weight);
                    relationUpdated = true;
                }

                if (relationUpdated)
                {
                    InvokeRelationUpdated(_selectedRelation);
                }
            }
        }

        private void AcceptAddExecute(object? parameter)
        {
            if (_selectedConsumer != null && _selectedProvider != null)
            {
                IRelation? createdRelation = _relationEditing.CreateRelation(_selectedConsumer, _selectedProvider,
                    SelectedRelationType, Weight);
                if (createdRelation != null)
                {
                    InvokeRelationUpdated(createdRelation);
                }
            }
        }

        private bool AcceptCanExecute(object? parameter)
        {
            if (_selectedConsumer == null)
            {
                Help = "No consumer selected";
                return false;
            }
            else if (_selectedProvider == null)
            {
                Help = "No provider selected";
                return false;
            }
            else if (_selectedConsumer == _selectedProvider)
            {
                Help = "Can not connect to itself";
                return false;
            }
            else if (_selectedConsumer.IsRecursiveChildOf(_selectedProvider))
            {
                Help = "Can not connect to child";
                return false;
            }
            else if (_selectedProvider.IsRecursiveChildOf(_selectedConsumer))
            {
                Help = "Can not connect to child";
                return false;
            }
            else if (Weight < 0)
            {
                Help = "Weight can not be negative";
                return false;
            }
            else if (Weight == 0)
            {
                Help = "Weight can not be zero";
                return false;
            }
            else
            {
                Help = "";
                return true;
            }
        }

        private void InvokeRelationUpdated(IRelation updateRelation)
        {
            RelationUpdated?.Invoke(this, updateRelation);
        }
    }
}
