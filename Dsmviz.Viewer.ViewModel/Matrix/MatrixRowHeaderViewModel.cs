using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using System.Collections.ObjectModel;
using Dsmviz.Interfaces.ViewModel.Matrix;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class MatrixRowHeaderViewModel(IMatrixViewModel viewModel, IElementQuery elementQuery)
        : ViewModelBase, IMatrixRowHeaderViewModel
    {
        private IMatrixRowHeaderTreeItemViewModel? _selectedTreeItem;
        private IMatrixRowHeaderTreeItemViewModel? _hoveredTreeItem;
        private ObservableCollection<IMatrixRowHeaderTreeItemViewModel> _elementViewModelTree = [];
        private List<IMatrixRowHeaderTreeItemViewModel> _elementViewModelLeafs = [];

        public IReadOnlyList<IMatrixRowHeaderTreeItemViewModel> Reload()
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

        public ObservableCollection<IMatrixRowHeaderTreeItemViewModel> ElementViewModelTree
        {
            get => _elementViewModelTree;
            private set { _elementViewModelTree = value; OnPropertyChanged(); }
        }

        public void HoverTreeItem(IMatrixRowHeaderTreeItemViewModel hoveredTreeItem)
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

        public void SelectTreeItem(IMatrixRowHeaderTreeItemViewModel selectedTreeItem)
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

        public IMatrixRowHeaderTreeItemViewModel? SelectedTreeItem
        {
            get
            {
                IMatrixRowHeaderTreeItemViewModel? selectedTreeItem;
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

        public IMatrixRowHeaderTreeItemViewModel? HoveredTreeItem
        {
            get
            {
                IMatrixRowHeaderTreeItemViewModel? hoveredTreeItem;
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

        private ObservableCollection<IMatrixRowHeaderTreeItemViewModel> CreateElementViewModelTree()
        {
            int depth = 0;
            ObservableCollection<IMatrixRowHeaderTreeItemViewModel> tree = [];

            if (elementQuery.RootElement.HasChildren)
            {
                MatrixRowHeaderTreeItemViewModel childViewModel = new MatrixRowHeaderTreeItemViewModel(viewModel, elementQuery.RootElement, depth);
                tree.Add(childViewModel);
                AddElementViewModelChildren(childViewModel);
            }

            return tree;
        }

        private void AddElementViewModelChildren(IMatrixRowHeaderTreeItemViewModel parentViewModel)
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

        private List<IMatrixRowHeaderTreeItemViewModel> FindLeafElementViewModels()
        {
            List<IMatrixRowHeaderTreeItemViewModel> leafViewModels = [];

            foreach (IMatrixRowHeaderTreeItemViewModel childViewModel in ElementViewModelTree)
            {
                FindLeafElementViewModels(leafViewModels, childViewModel);
            }

            return leafViewModels;
        }

        private void FindLeafElementViewModels(List<IMatrixRowHeaderTreeItemViewModel> leafViewModels, IMatrixRowHeaderTreeItemViewModel parentViewModel)
        {
            if (!parentViewModel.IsExpanded)
            {
                leafViewModels.Add(parentViewModel);
            }

            foreach (IMatrixRowHeaderTreeItemViewModel childViewModel in parentViewModel.Children)
            {
                FindLeafElementViewModels(leafViewModels, childViewModel);
            }
        }
    }
}
