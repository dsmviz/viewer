using System.ComponentModel;

namespace Dsmviz.Interfaces.ViewModel.Matrix
{
    public interface IMatrixRowMetricsViewModel : INotifyPropertyChanged
    {
        void HoverRow(int? row);
        int? HoveredRow { get; }

        void SelectRow(int? row);
        int? SelectedRow { get; }

        int RowCount { get; }

        int GetRowDepth(int row);
        string GetRowMetric(int row);
    }
}
