using Dsmviz.Interfaces.Application.Metrics;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class MatrixRowMetricsViewModel(IMatrixViewModel matrixViewModel, IMetrics applicationMetrics)
        : ViewModelBase, IMatrixRowMetricsViewModel
    {
        private IReadOnlyList<MatrixRowHeaderTreeItemViewModel> _elementViewModelLeafs = [];

        private MetricType _selectedMetricType = MetricType.NumberOfElements;

        public void Reload(MetricType selectedMetricType, IReadOnlyList<MatrixRowHeaderTreeItemViewModel> elementViewModelLeafs)
        {
            _elementViewModelLeafs = elementViewModelLeafs;
            _selectedMetricType = selectedMetricType;
            OnPropertyChanged(nameof(MatrixRowMetricsViewModel));
        }

        public void Redraw()
        {
            OnPropertyChanged(nameof(MatrixRowMetricsViewModel));
        }

        public int RowCount => _elementViewModelLeafs.Count;

        public int GetRowDepth(int row)
        {
            return _elementViewModelLeafs[row].Depth;
        }

        public string GetRowMetric(int row)
        {
            IMetric? metric = applicationMetrics.GetMetric(_selectedMetricType, _elementViewModelLeafs[row].Element);
            return metric != null ? metric.FormattedValue : "";
        }

        public void HoverRow(int? row)
        {
            matrixViewModel.HoverRow(row);
        }

        public int? HoveredRow => matrixViewModel.HoveredRow;

        public void SelectRow(int? row)
        {
            matrixViewModel.SelectRow(row);
        }

        public int? SelectedRow => matrixViewModel.SelectedRow;
    }
}
