using Dsmviz.Interfaces.Application.Metrics;
using Dsmviz.Viewer.ViewModel.Common;
using System.Windows.Input;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class MatrixMetricsSelectorViewModel : ViewModelBase, IMatrixMetricsSelectorViewModel
    {
        private readonly Dictionary<MetricType, string> _metricTypeNames;
        private string _selectedMetricTypeName;
        private MetricType _selectedMetricType;

        public event EventHandler? SelectedMetricChanged;

        public MatrixMetricsSelectorViewModel()
        {
            _metricTypeNames = new Dictionary<MetricType, string>
            {
                [MetricType.NumberOfElements] = "Internal\nElements",
                [MetricType.RelativeSizePercentage] = "Relative\nSize",
                [MetricType.IngoingRelations] = "Ingoing Relations",
                [MetricType.OutgoingRelations] = "Outgoing\nRelations",
                [MetricType.InternalRelations] = "Internal\nRelations",
                [MetricType.ExternalRelations] = "External\nRelations",
                [MetricType.HierarchicalCycles] = "Hierarchical\nCycles",
                [MetricType.SystemCycles] = "System\nCycles",
                [MetricType.Cycles] = "Total\nCycles",
                [MetricType.CycalityPercentage] = "Total\nCycality",
            };

            _selectedMetricType = MetricType.NumberOfElements;
            _selectedMetricTypeName = _metricTypeNames[_selectedMetricType];

            PreviousMetricCommand = RegisterCommand(PreviousMetricExecute, PreviousMetricCanExecute);
            NextMetricCommand = RegisterCommand(NextMetricExecute, NextMetricCanExecute);
        }

        public string SelectedMetricTypeName
        {
            get => _selectedMetricTypeName;
            set
            {
                _selectedMetricTypeName = value;
                _selectedMetricType = _metricTypeNames.FirstOrDefault(x => x.Value == _selectedMetricTypeName).Key;
                OnPropertyChanged();
                SelectedMetricChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public MetricType SelectedMetricType
        {
            get => _selectedMetricType;
            set
            {
                _selectedMetricType = value;
                OnPropertyChanged(); // Updating this property triggers redraw
            }
        }

        public ICommand PreviousMetricCommand { get; }
        public ICommand NextMetricCommand { get; }

        private void PreviousMetricExecute(object? parameter)
        {
            _selectedMetricType--;
            SelectedMetricTypeName = _metricTypeNames[_selectedMetricType];

            NotifyCommandsCanExecuteChanged();
        }

        private bool PreviousMetricCanExecute(object? parameter)
        {
            return _selectedMetricType != MetricType.NumberOfElements;
        }

        private void NextMetricExecute(object? parameter)
        {
            _selectedMetricType++;
            SelectedMetricTypeName = _metricTypeNames[_selectedMetricType];

            NotifyCommandsCanExecuteChanged();
        }

        private bool NextMetricCanExecute(object? parameter)
        {
            return _selectedMetricType != MetricType.ExternalRelations;
        }
    }
}
