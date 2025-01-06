using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using System.Collections.ObjectModel;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class MatrixRowHeaderViewModel(IMatrixViewModel viewModel, IElementQuery elementQuery)
        : ViewModelBase, IMatrixRowHeaderViewModel
    {
        private MatrixRowHeaderTreeItemViewModel? _selectedTreeItem;
        private MatrixRowHeaderTreeItemViewModel? _hoveredTreeItem;
        private ObservableCollection<MatrixRowHeaderTreeItemViewModel> _elementViewModelTree = [];
        private List<MatrixRowHeaderTreeItemViewModel> _elementViewModelLeafs = [];

        public IReadOnlyList<MatrixRowHeaderTreeItemViewModel> Reload()
        {
            ElementViewModelTree = CreateElementViewModelTree();
            _elementViewModelLeafs = FindLeafElementViewModels();

            OnPropertyChanged(nameof(MatrixRowHeaderViewModel));

            return _elementViewModelLeafs;
        }

        public void Redraw()
        {
            OnPropertyChanged(nameof(MatrixRowHeaderViewModel));
        }

        public void ContentChanged()
        {
            viewModel.ContentChanged();
        }

        public ObservableCollection<MatrixRowHeaderTreeItemViewModel> ElementViewModelTree
        {
            get => _elementViewModelTree;
            private set { _elementViewModelTree = value; OnPropertyChanged(); }
        }

        public void HoverTreeItem(MatrixRowHeaderTreeItemViewModel hoveredTreeItem)
        {
            viewModel.HoverCell(null, null);
            for (int row = 0; row < _elementViewModelLeafs.Count; row++)
            {
                if (_elementViewModelLeafs[row] == hoveredTreeItem)
                {
                    viewModel.HoverRow(row);
                }
            }
            _hoveredTreeItem = hoveredTreeItem;
        }

        public void SelectTreeItem(MatrixRowHeaderTreeItemViewModel selectedTreeItem)
        {
            viewModel.SelectCell(null, null);
            for (int row = 0; row < _elementViewModelLeafs.Count; row++)
            {
                if (_elementViewModelLeafs[row].Id == selectedTreeItem.Id)
                {
                    viewModel.SelectRow(row);
                }
            }
            _selectedTreeItem = selectedTreeItem;
        }

        public MatrixRowHeaderTreeItemViewModel? SelectedTreeItem
        {
            get
            {
                MatrixRowHeaderTreeItemViewModel? selectedTreeItem;
                if (viewModel.SelectedRow.HasValue && (viewModel.SelectedRow.Value < _elementViewModelLeafs.Count))
                {
                    selectedTreeItem = _elementViewModelLeafs[viewModel.SelectedRow.Value];
                }
                else
                {
                    selectedTreeItem = _selectedTreeItem;
                }
                return selectedTreeItem;
            }
        }

        public MatrixRowHeaderTreeItemViewModel? HoveredTreeItem
        {
            get
            {
                MatrixRowHeaderTreeItemViewModel? hoveredTreeItem;
                if (viewModel.HoveredRow.HasValue && (viewModel.HoveredRow.Value < _elementViewModelLeafs.Count))
                {
                    hoveredTreeItem = _elementViewModelLeafs[viewModel.HoveredRow.Value];
                }
                else
                {
                    hoveredTreeItem = _hoveredTreeItem;
                }
                return hoveredTreeItem;
            }
        }

        private ObservableCollection<MatrixRowHeaderTreeItemViewModel> CreateElementViewModelTree()
        {
            int depth = 0;
            ObservableCollection<MatrixRowHeaderTreeItemViewModel> tree = [];

            if (elementQuery.RootElement.HasChildren)
            {
                MatrixRowHeaderTreeItemViewModel childViewModel = new MatrixRowHeaderTreeItemViewModel(viewModel, elementQuery.RootElement, depth);
                tree.Add(childViewModel);
                AddElementViewModelChildren(childViewModel);
            }

            return tree;
        }

        private void AddElementViewModelChildren(MatrixRowHeaderTreeItemViewModel parentViewModel)
        {
            if (parentViewModel.Element.IsExpanded)
            {
                foreach (IElement childElement in parentViewModel.Element.Children)
                {
                    MatrixRowHeaderTreeItemViewModel childViewModel = new MatrixRowHeaderTreeItemViewModel(viewModel, childElement, parentViewModel.Depth + 1);
                    parentViewModel.AddChild(childViewModel);
                    AddElementViewModelChildren(childViewModel);
                }
            }
            else
            {
                parentViewModel.ClearChildren();
            }
        }

        private List<MatrixRowHeaderTreeItemViewModel> FindLeafElementViewModels()
        {
            List<MatrixRowHeaderTreeItemViewModel> leafViewModels = [];

            foreach (MatrixRowHeaderTreeItemViewModel childViewModel in ElementViewModelTree)
            {
                FindLeafElementViewModels(leafViewModels, childViewModel);
            }

            return leafViewModels;
        }

        private void FindLeafElementViewModels(List<MatrixRowHeaderTreeItemViewModel> leafViewModels, MatrixRowHeaderTreeItemViewModel parentViewModel)
        {
            if (!parentViewModel.IsExpanded)
            {
                leafViewModels.Add(parentViewModel);
            }

            foreach (MatrixRowHeaderTreeItemViewModel childViewModel in parentViewModel.Children)
            {
                FindLeafElementViewModels(leafViewModels, childViewModel);
            }
        }
    }
}
