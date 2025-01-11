using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Application.Metrics;
using Dsmviz.Interfaces.Application.Persistency;
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using Dsmviz.Viewer.ViewModel.Main;
using Dsmviz.Viewer.ViewModel.SideBar;
using System.Windows.Input;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Main;
using Dsmviz.Interfaces.ViewModel.Matrix;
using Dsmviz.Interfaces.ViewModel.Sidebar;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class MatrixViewModel : ViewModelBase, IMatrixViewModel
    {
        private double _zoomLevel;
        private readonly IMainViewModel _mainViewModel;
        private IReadOnlyList<IMatrixRowHeaderTreeItemViewModel> _elementViewModelLeafs = [];

        private int? _hoveredRow;
        private int? _hoveredColumn;
        private int _matrixSize;
        private bool _isMetricsViewExpanded;
        private bool _isEditViewExpanded;

        private int? _selectedConsumerId;
        private int? _selectedProviderId;

        private readonly ModelSideBarViewModel _modelInfoViewModel;
        private readonly MatrixElementSideBarViewModel _elementInfoViewModel;
        private readonly MatrixCellSideBarViewModel _relationInfoViewModel;

        private readonly MatrixRowMetricsViewModel _matrixRowMetricsViewModel;
        private readonly MatrixMetricsSelectorViewModel _matrixMetricsSelectorViewModel;
        private readonly MatrixColumnHeaderViewModel _matrixColumnHeaderViewModel;

        private readonly MatrixCellsViewModel _matrixCellsViewModel;
        private readonly MatrixRowHeaderViewModel _matrixRowHeaderViewModel;

        public MatrixViewModel(IMainViewModel mainViewModel,
            IMatrix applicationMatrix,
            IMetrics applicationMetrics,
            IPersistency persistency,
            IEditing editing,
            IQuery query)
        {
            _mainViewModel = mainViewModel;

            var elementEditing = editing.ElementEditing;
            var elementQuery = query.ElementQuery;
            var relationEditing = editing.RelationEditing;
            var relationQuery = query.RelationQuery;

            _matrixRowHeaderViewModel = new MatrixRowHeaderViewModel(this, query.ElementQuery);
            _matrixColumnHeaderViewModel = new MatrixColumnHeaderViewModel(this);
            _matrixCellsViewModel = new MatrixCellsViewModel(this, applicationMatrix.DependencyWeightMatrix,
                applicationMatrix.DependencyCycleMatrix);

            _matrixMetricsSelectorViewModel = new MatrixMetricsSelectorViewModel();
            _matrixMetricsSelectorViewModel.SelectedMetricChanged += OnSelectedMetricChanged;
            _matrixRowMetricsViewModel = new MatrixRowMetricsViewModel(this, applicationMetrics);

            _modelInfoViewModel = new ModelSideBarViewModel(persistency);
            _elementInfoViewModel = new MatrixElementSideBarViewModel(elementEditing, elementQuery, relationEditing, relationQuery, applicationMatrix);
            _relationInfoViewModel = new MatrixCellSideBarViewModel(relationEditing, relationQuery, applicationMatrix);

            ToggleMetricsViewExpandedCommand = RegisterCommand(ToggleMetricsViewExpandedExecute, ToggleMetricsViewExpandedCanExecute);
            ToggleEditViewExpandedCommand = RegisterCommand(ToggleEditViewExpandedExecute, ToggleEditViewExpandedCanExecute);

            IsMetricsViewExpanded = true;
            IsEditViewExpanded = true;

            ZoomLevel = 1.0;

            Reload();
        }

        public IModelSideBarViewModel ModelInfoViewModel => _modelInfoViewModel;
        public IMatrixElementSideBarViewModel ElementInfoViewModel => _elementInfoViewModel;
        public IMatrixCellSideBarViewModel RelationInfoViewModel => _relationInfoViewModel;

        public IMatrixMetricsSelectorViewModel MatrixMetricsSelectorViewModel => _matrixMetricsSelectorViewModel;
        public IMatrixRowMetricsViewModel MatrixRowMetricsViewModel => _matrixRowMetricsViewModel;

        public IMatrixRowHeaderViewModel MatrixRowHeaderViewModel => _matrixRowHeaderViewModel;
        public IMatrixColumnHeaderViewModel MatrixColumnHeaderViewModel => _matrixColumnHeaderViewModel;
        public IMatrixCellsViewModel MatrixCellsViewModel => _matrixCellsViewModel;

        public ViewPerspective SelectedViewPerspective => _mainViewModel.SelectedViewPerspective;

        // TreeItemViewModel/MatrixViewModel
        public ICommand ChangeElementParentCommand => _mainViewModel.ChangeElementParentCommand;

        // TopCornerView
        public ICommand ToggleMetricsViewExpandedCommand { get; }

        // Sidebar
        public ICommand ToggleEditViewExpandedCommand { get; }

        public int MatrixSize
        {
            get => _matrixSize;
            set { _matrixSize = value; OnPropertyChanged(); }
        }

        public bool IsMetricsViewExpanded
        {
            get => _isMetricsViewExpanded;
            set { _isMetricsViewExpanded = value; OnPropertyChanged(); }
        }

        public bool IsEditViewExpanded
        {
            get => _isEditViewExpanded;
            set { _isEditViewExpanded = value; OnPropertyChanged(); }
        }

        public double ZoomLevel
        {
            get => _zoomLevel;
            set { _zoomLevel = value; OnPropertyChanged(); }
        }

        private void Reload()
        {
            SelectInfoPerspective();

            BackupSelectionBeforeReload();
            _elementViewModelLeafs = _matrixRowHeaderViewModel.Reload();
            _matrixColumnHeaderViewModel.Reload(_elementViewModelLeafs);
            _matrixCellsViewModel.Reload(_elementViewModelLeafs);
            _matrixRowMetricsViewModel.Reload(MatrixMetricsSelectorViewModel.SelectedMetricType, _elementViewModelLeafs);
            MatrixSize = _elementViewModelLeafs.Count;
            RestoreSelectionAfterReload();
        }

        private void Redraw()
        {
            _matrixRowHeaderViewModel.Redraw();
            _matrixColumnHeaderViewModel.Redraw();
            _matrixCellsViewModel.Redraw();
            _matrixRowMetricsViewModel.Redraw();
        }

        public void ContentChanged()
        {
            Reload();
        }

        private void OnSelectedMetricChanged(object? sender, EventArgs e)
        {
            Reload();
        }

        public void SelectRow(int? row)
        {
            if (SelectedRow != row || SelectedColumn != null)
            {
                SelectedRow = row;
                SelectedColumn = null;

                // Only redraw when change to avoid excessive redraws of the matrix visualization
                UpdateProviderRows();
                UpdateConsumerRows();
                SelectInfoPerspective();
                Redraw();

                _mainViewModel.UpdateCommandStates();
            }
        }

        public void SelectColumn(int? column)
        {
            if (SelectedRow != null || SelectedColumn != column)
            {
                SelectedRow = null;
                SelectedColumn = column;

                // Only redraw when change to avoid excessive redraws of the matrix visualization
                UpdateProviderRows();
                UpdateConsumerRows();
                SelectInfoPerspective();
                Redraw();

                _mainViewModel.UpdateCommandStates();
            }
        }

        public void SelectCell(int? row, int? column)
        {
            if (SelectedRow != row || SelectedColumn != column)
            {
                SelectedRow = row;
                SelectedColumn = column;

                // Only redraw when change to avoid excessive redraws of the matrix visualization
                UpdateProviderRows();
                UpdateConsumerRows();
                SelectInfoPerspective();
                Redraw();

                _mainViewModel.UpdateCommandStates();
            }
        }

        public IElement? SelectedProvider
        {
            get
            {
                IElement? selectedProvider = null;
                if (SelectedRow.HasValue)
                {
                    selectedProvider = _elementViewModelLeafs[SelectedRow.Value].Element;
                }
                return selectedProvider;
            }
        }

        public IElement? SelectedConsumer
        {
            get
            {
                IElement? selectedConsumer = null;
                if (SelectedColumn.HasValue)
                {
                    selectedConsumer = _elementViewModelLeafs[SelectedColumn.Value].Element;
                }
                return selectedConsumer;
            }
        }

        private void SelectInfoPerspective()
        {
            ModelInfoViewModel.Unselect();
            ElementInfoViewModel.Unselect();
            RelationInfoViewModel.Unselect();

            if (SelectedConsumer != null && SelectedProvider != null)
            {
                RelationInfoViewModel.SelectCell(SelectedConsumer, SelectedProvider);
            }
            else if (SelectedProvider != null)
            {
                ElementInfoViewModel.SelectRow(SelectedProvider);
            }
            else if (SelectedConsumer != null)
            {
                ElementInfoViewModel.SelectColumn(SelectedConsumer);
            }
            else
            {
                ModelInfoViewModel.Select();
            }
        }

        public int? SelectedRow { get; private set; }

        public int? SelectedColumn { get; private set; }

        public void HoverRow(int? row)
        {
            if (HoveredRow != row || HoveredColumn != null)
            {
                HoveredRow = row;
                HoveredColumn = null;

                // TODO FIX: Redraw break arrow click in rows
                // Only redraw when change to avoid excessive redraws of the matrix visualization
                //Redraw();
            }
        }

        public void HoverColumn(int? column)
        {
            if (HoveredRow != null || HoveredColumn != column)
            {
                HoveredRow = null;
                HoveredColumn = column;

                // TODO FIX: Redraw break arrow click in rows
                // Only redraw when change to avoid excessive redraws of the matrix visualization
                //Redraw();
            }
        }

        public void HoverCell(int? row, int? column)
        {
            if (HoveredRow != row || HoveredColumn != column)
            {
                HoveredRow = row;
                HoveredColumn = column;

                // TODO FIX: Redraw break arrow click in rows
                // Only redraw when change to avoid excessive redraws of the matrix visualization
                //Redraw();
            }
        }

        public int? HoveredRow
        {
            get => _hoveredRow;
            private set { _hoveredRow = value; OnPropertyChanged(); }
        }

        public int? HoveredColumn
        {
            get => _hoveredColumn;
            private set { _hoveredColumn = value; OnPropertyChanged(); }
        }

        public bool IsSearchActive => _mainViewModel.IsSearchActive;

        private void ToggleMetricsViewExpandedExecute(object? parameter)
        {
            IsMetricsViewExpanded = !IsMetricsViewExpanded;
        }

        private bool ToggleMetricsViewExpandedCanExecute(object? parameter)
        {
            return true;
        }

        private void ToggleEditViewExpandedExecute(object? parameter)
        {
            IsEditViewExpanded = !IsEditViewExpanded;
        }

        private bool ToggleEditViewExpandedCanExecute(object? parameter)
        {
            return true;
        }

        private bool IsCellSelected()
        {
            return SelectedConsumer != null && SelectedProvider != null;
        }

        private bool IsDiagonalCellSelected()
        {
            return SelectedConsumer != null && SelectedProvider != null && SelectedConsumer == SelectedProvider;
        }

        private bool IsRowSelected()
        {
            return SelectedProvider != null;
        }

        private bool IsColumnSelected()
        {
            return SelectedConsumer != null;
        }

        private void UpdateProviderRows()
        {
            if (SelectedRow.HasValue)
            {
                for (int row = 0; row < _elementViewModelLeafs.Count; row++)
                {
                    _elementViewModelLeafs[row].IsProvider = _matrixCellsViewModel.GetCellWeight(row, SelectedRow.Value) > 0;
                }
            }
            else
            {
                foreach (var elementViewModelLeaf in _elementViewModelLeafs)
                {
                    elementViewModelLeaf.IsProvider = false;
                }
            }
        }

        private void UpdateConsumerRows()
        {
            if (SelectedRow.HasValue)
            {
                for (int row = 0; row < _elementViewModelLeafs.Count; row++)
                {
                    _elementViewModelLeafs[row].IsConsumer = _matrixCellsViewModel.GetCellWeight(SelectedRow.Value, row) > 0;
                }
            }
            else
            {
                foreach (var elementViewModelLeaf in _elementViewModelLeafs)
                {
                    elementViewModelLeaf.IsConsumer = false;
                }
            }
        }

        private void BackupSelectionBeforeReload()
        {
            _selectedConsumerId = SelectedConsumer?.Id;
            _selectedProviderId = SelectedProvider?.Id;
        }

        private void RestoreSelectionAfterReload()
        {
            for (int i = 0; i < _elementViewModelLeafs.Count; i++)
            {
                if (_selectedProviderId.HasValue && (_selectedProviderId.Value == _elementViewModelLeafs[i].Id))
                {
                    SelectRow(i);
                }

                if (_selectedConsumerId.HasValue && (_selectedConsumerId.Value == _elementViewModelLeafs[i].Id))
                {
                    SelectColumn(i);
                }
            }
        }
    }
}
