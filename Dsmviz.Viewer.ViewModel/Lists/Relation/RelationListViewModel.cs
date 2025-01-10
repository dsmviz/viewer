using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using Dsmviz.Viewer.ViewModel.Editing.Relation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Dsmviz.ViewModel.Interfaces.Editing.Relation;
using Dsmviz.ViewModel.Interfaces.Lists.Relation;

namespace Dsmviz.Viewer.ViewModel.Lists.Relation
{
    public class RelationListViewModel : ViewModelBase, IRelationListViewModel
    {
        private readonly RelationListViewModelType _viewModelType;
        private readonly IRelationQuery _relationQuery;
        private readonly IRelationEditing _relationEditing;
        private readonly IElement _selectedConsumer;
        private readonly IElement _selectedProvider;
        private ObservableCollection<IRelationListItemViewModel> _relations = [];
        private IRelationListItemViewModel? _selectedRelation;

        public event EventHandler<IRelationEditViewModel>? RelationAddStarted;
        public event EventHandler<IRelationEditViewModel>? RelationEditStarted;

        public RelationListViewModel(RelationListViewModelType viewModelType, IRelationQuery relationQuery, IRelationEditing relationEditing, IElement selectedConsumer, IElement selectedProvider)
        {
            _viewModelType = viewModelType;
            _relationQuery = relationQuery;
            _relationEditing = relationEditing;
            _selectedConsumer = selectedConsumer;
            _selectedProvider = selectedProvider;

            Title = "Relation List";
            switch (viewModelType)
            {
                case RelationListViewModelType.ElementIngoingRelations:
                    SubTitle = $"Ingoing relations of:\n -{_selectedProvider.Fullname}";
                    break;
                case RelationListViewModelType.ElementOutgoingRelations:
                    SubTitle = $"Outgoing relations of:\n -{_selectedProvider.Fullname}";
                    break;
                case RelationListViewModelType.ElementInternalRelations:
                    SubTitle = $"Internal relations of:\n -{_selectedProvider.Fullname}";
                    break;
                case RelationListViewModelType.ConsumerProviderRelations:
                    SubTitle = $"Relations from:\n -{_selectedConsumer.Fullname}\nto\n: -{_selectedProvider.Fullname}";
                    break;
                default:
                    SubTitle = "";
                    break;
            }

            AddRelationCommand = RegisterCommand(AddConsumerProviderRelationExecute, AddRelationCanExecute);
            CopyToClipboardCommand = RegisterCommand(CopyToClipboardExecute);
            DeleteRelationCommand = RegisterCommand(DeleteRelationExecute, DeleteRelationCanExecute);
            EditRelationCommand = RegisterCommand(EditRelationExecute, EditRelationCanExecute);

            UpdateRelations(null);
        }

        public string Title { get; }
        public string SubTitle { get; }

        public ObservableCollection<IRelationListItemViewModel> Relations
        {
            get => _relations;
            private set { _relations = value; OnPropertyChanged(); }
        }

        public IRelationListItemViewModel? SelectedRelation
        {
            get => _selectedRelation;
            set { _selectedRelation = value; OnPropertyChanged(); }
        }

        public ICommand CopyToClipboardCommand { get; }
        public ICommand DeleteRelationCommand { get; }
        public ICommand EditRelationCommand { get; }
        public ICommand AddRelationCommand { get; }

        private void CopyToClipboardExecute(object? parameter)
        {
            if (Relations.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                StringBuilder headerLine = new StringBuilder();
                headerLine.Append("Index,");
                headerLine.Append("ConsumerPath,");
                headerLine.Append("ConsumerName,");
                headerLine.Append("ProviderPath,");
                headerLine.Append("ProviderName,");
                headerLine.Append("Type,");
                headerLine.Append("Weight,");
                headerLine.Append("Cyclic,");
                builder.AppendLine(headerLine.ToString());

                foreach (RelationListItemViewModel viewModel in Relations)
                {
                    StringBuilder line = new StringBuilder();
                    line.Append($"{viewModel.Index},");
                    line.Append($"{viewModel.ConsumerPath},");
                    line.Append($"{viewModel.ConsumerName},");
                    line.Append($"{viewModel.ProviderPath},");
                    line.Append($"{viewModel.ProviderName},");
                    line.Append($"{viewModel.RelationType},");
                    line.Append($"{viewModel.RelationWeight},");
                    builder.AppendLine(line.ToString());
                }
                // TODO FIX: Clipboard.SetText(builder.ToString());
            }
        }

        private void DeleteRelationExecute(object? parameter)
        {
            if (SelectedRelation != null)
            {
                _relationEditing.DeleteRelation(SelectedRelation.Relation);
                UpdateRelations(SelectedRelation.Relation);
            }
        }

        private bool DeleteRelationCanExecute(object? parameter)
        {
            return SelectedRelation != null;
        }

        private void EditRelationExecute(object? parameter)
        {
            if (SelectedRelation != null)
            {
                RelationEditViewModel relationEditViewModel = new RelationEditViewModel(RelationEditViewModelType.Modify, _relationEditing, _relationQuery, SelectedRelation.Relation, null, null);
                relationEditViewModel.RelationUpdated += OnRelationUpdated;
                RelationEditStarted?.Invoke(this, relationEditViewModel);
            }
        }

        private bool EditRelationCanExecute(object? parameter)
        {
            return SelectedRelation != null;
        }

        private void AddConsumerProviderRelationExecute(object? parameter)
        {
            RelationEditViewModel relationEditViewModel = new RelationEditViewModel(RelationEditViewModelType.Add, _relationEditing, _relationQuery, null, _selectedConsumer, _selectedProvider);
            relationEditViewModel.RelationUpdated += OnRelationUpdated;
            RelationAddStarted?.Invoke(this, relationEditViewModel);
        }

        private bool AddRelationCanExecute(object? parameter)
        {
            return true;
        }

        private void OnRelationUpdated(object? sender, IRelation updatedRelation)
        {
            UpdateRelations(updatedRelation);
        }

        private void UpdateRelations(IRelation? updatedRelation)
        {
            RelationListItemViewModel? selectedRelationListItemViewModel = null;
            IEnumerable<IRelation> relations;
            switch (_viewModelType)
            {
                case RelationListViewModelType.ElementIngoingRelations:
                    relations = _relationQuery.GetAllIngoingRelations(_selectedProvider);
                    break;
                case RelationListViewModelType.ElementOutgoingRelations:
                    relations = _relationQuery.GetAllOutgoingRelations(_selectedProvider);
                    break;
                case RelationListViewModelType.ElementInternalRelations:
                    relations = _relationQuery.GetAllInternalRelations(_selectedProvider);
                    break;
                case RelationListViewModelType.ConsumerProviderRelations:
                    relations = _relationQuery.GetAllRelations(_selectedConsumer, _selectedProvider);
                    break;
                default:
                    relations = new List<IRelation>();
                    break;
            }

            List<RelationListItemViewModel> relationViewModels = [];

            foreach (IRelation relation in relations)
            {
                RelationListItemViewModel relationListItemViewModel = new RelationListItemViewModel(relation);
                relationViewModels.Add(relationListItemViewModel);

                if (updatedRelation != null)
                {
                    if (relation.Id == updatedRelation.Id)
                    {
                        selectedRelationListItemViewModel = relationListItemViewModel;
                    }
                }
            }

            relationViewModels.Sort();

            int index = 1;
            foreach (RelationListItemViewModel viewModel in relationViewModels)
            {
                viewModel.Index = index;
                index++;
            }

            Relations = new ObservableCollection<IRelationListItemViewModel>(relationViewModels);
            SelectedRelation = selectedRelationListItemViewModel;
        }
    }
}
