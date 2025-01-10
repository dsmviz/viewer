using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using Dsmviz.Viewer.ViewModel.Tooltips;
using Dsmviz.ViewModel.Interfaces.Matrix;
using Dsmviz.ViewModel.Interfaces.Tooltips;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class MatrixColumnHeaderViewModel(IMatrixViewModel viewModel) : ViewModelBase, IMatrixColumnHeaderViewModel
    {
        private IReadOnlyList<IMatrixRowHeaderTreeItemViewModel> _elementViewModelLeafs = [];
        private IElementToolTipViewModel? _columnHeaderTooltipViewModel;

        public void Reload(IReadOnlyList<IMatrixRowHeaderTreeItemViewModel> elementViewModelLeafs)
        {
            _elementViewModelLeafs = elementViewModelLeafs;
            OnPropertyChanged(nameof(MatrixColumnHeaderViewModel));
        }

        public void Redraw()
        {
            OnPropertyChanged(nameof(MatrixColumnHeaderViewModel));
        }

        public int ColumnCount => _elementViewModelLeafs.Count;

        public void HoverColumn(int? column)
        {
            viewModel.HoverColumn(column);
            UpdateColumnHeaderTooltip(column);
        }

        public int? HoveredColumn => viewModel.HoveredColumn;

        public void SelectColumn(int? column)
        {
            viewModel.SelectColumn(column);
        }

        public int? SelectedColumn => viewModel.SelectedColumn;

        public int GetColumnDepth(int column)
        {
            return _elementViewModelLeafs[column].Depth;
        }

        public string GetColumnContent(int column)
        {
            return _elementViewModelLeafs[column].Element.Order.ToString();
        }

        public IElementToolTipViewModel? ColumnHeaderToolTipViewModel
        {
            get => _columnHeaderTooltipViewModel;
            set { _columnHeaderTooltipViewModel = value; OnPropertyChanged(); }
        }

        private void UpdateColumnHeaderTooltip(int? column)
        {
            if (column.HasValue)
            {
                IElement element = _elementViewModelLeafs[column.Value].Element;
                ColumnHeaderToolTipViewModel = new ElementToolTipViewModel(element);
            }
        }
    }
}
