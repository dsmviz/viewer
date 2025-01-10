using System.ComponentModel;
using Dsmviz.Interfaces.Application.Metrics;

namespace Dsmviz.ViewModel.Interfaces.Matrix
{
    public interface IMatrixMetricsSelectorViewModel : INotifyPropertyChanged
    {
        MetricType SelectedMetricType { get; }
    }
}
