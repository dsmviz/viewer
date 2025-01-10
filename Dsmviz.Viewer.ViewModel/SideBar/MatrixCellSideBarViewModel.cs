

using System.ComponentModel;
using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using Dsmviz.Viewer.ViewModel.Lists.Relation;
using System.Windows.Input;
using Dsmviz.ViewModel.Interfaces.Lists.Relation;
using Dsmviz.ViewModel.Interfaces.Sidebar;

namespace Dsmviz.Viewer.ViewModel.SideBar
{
    public class MatrixCellSideBarViewModel : ViewModelBase, IMatrixCellSideBarViewModel
    {
        // Interfaces
        private readonly IRelationEditing _relationEditing;
        private readonly IRelationQuery _relationQuery;
        private readonly IMatrix _matrix;

        // Matrix selection
        private bool _selected;
        private IElement? _selectedConsumer;
        private IElement? _selectedProvider;
        private int _cellDerivedWeight;
        private string _cellCycle;

        // Relation count
        private int _relationCount;

        private void NotifyRelationsReportReady(RelationListViewModelType viewModelType, IElement selectedConsumer, IElement selectedProvider)
        {
            RelationListViewModel viewModel = new RelationListViewModel(viewModelType, _relationQuery, _relationEditing, selectedConsumer, selectedProvider);
            RelationsReportReady?.Invoke(this, viewModel);
        }

        public event EventHandler<IRelationListViewModel>? RelationsReportReady;

        public MatrixCellSideBarViewModel(IRelationEditing relationEditing, IRelationQuery relationQuery, IMatrix matrix)
        {
            _relationEditing = relationEditing;
            _relationQuery = relationQuery;
            _matrix = matrix;

            ShowRelationsListCommand = RegisterCommand(ShowRelationsListExecute, ShowRelationsListCanExecute);

            _relationCount = 0;
        }

        public void SelectCell(IElement selectedConsumer, IElement selectedProvider)
        {
            Selected = true;
            SelectedConsumer = selectedConsumer;
            SelectedProvider = selectedProvider;

            UpdateAfterSelection();
        }

        public void Unselect()
        {
            Selected = false;
            SelectedConsumer = null;
            SelectedProvider = null;
        }

        public bool Selected
        {
            get => _selected;
            private set { _selected = value; OnPropertyChanged(); }
        }

        public IElement? SelectedConsumer
        {
            get => _selectedConsumer;
            set { _selectedConsumer = value; OnPropertyChanged(); }
        }

        public IElement? SelectedProvider
        {
            get => _selectedProvider;
            set { _selectedProvider = value; OnPropertyChanged(); }
        }

        public int CellDerivedWeight
        {
            get => _cellDerivedWeight;
            private set { _cellDerivedWeight = value; OnPropertyChanged(); }
        }

        public string CellCycle
        {
            get => _cellCycle;
            private set { _cellCycle = value; OnPropertyChanged(); }
        }

        public int RelationCount
        {
            get => _relationCount;
            private set { _relationCount = value; OnPropertyChanged(); }
        }

        public ICommand ShowRelationsListCommand { get; }

        private void ShowRelationsListExecute(object? parameter)
        {
            if (SelectedConsumer != null && SelectedProvider != null)
            {
                NotifyRelationsReportReady(RelationListViewModelType.ConsumerProviderRelations, SelectedConsumer, SelectedProvider);
            }
        }

        private bool ShowRelationsListCanExecute(object? parameter)
        {
            return (SelectedConsumer != null && SelectedProvider != null);
        }

        private void UpdateAfterSelection()
        {
            if (_selectedConsumer != null && _selectedProvider != null)
            {
                CellDerivedWeight = _matrix.DependencyWeightMatrix.GetDerivedDependencyWeight(_selectedConsumer, _selectedProvider);
                Cycle cycleState = _matrix.DependencyCycleMatrix.GetCycle(_selectedConsumer, _selectedProvider);
                CellCycle = FormatCycleState(cycleState);
                RelationCount = _relationQuery.GetAllRelations(_selectedConsumer, _selectedProvider).Count();
            }

            NotifyCommandsCanExecuteChanged();
        }

        private string FormatCycleState(Cycle cycleState)
        {
            switch (cycleState)
            {
                case Cycle.System:
                    return "System Cycle";
                case Cycle.Hierarchical:
                    return "Hierarchical Cycle";
                case Cycle.HierarchicalContributor:
                    return "Hierarchical Cycle Contributor";
                case Cycle.None:
                    return "None";
                default:
                    return "?";
            }
        }
    }
}
