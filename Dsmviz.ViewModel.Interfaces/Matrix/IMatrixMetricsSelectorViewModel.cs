using System.ComponentModel;
using Dsmviz.Interfaces.Application.Metrics;

namespace Dsmviz.Interfaces.ViewModel.Matrix
{
    public interface IMatrixMetricsSelectorViewModel : INotifyPropertyChanged
    {
        MetricType SelectedMetricType { get; }
    }
}
