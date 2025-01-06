using Dsmviz.Interfaces.Application.Metrics;
using System.ComponentModel;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public interface IMatrixMetricsSelectorViewModel : INotifyPropertyChanged
    {
        MetricType SelectedMetricType { get; }
    }
}
