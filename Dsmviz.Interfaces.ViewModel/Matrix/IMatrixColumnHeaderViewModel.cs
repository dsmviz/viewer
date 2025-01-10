using System.ComponentModel;
using Dsmviz.Interfaces.ViewModel.Tooltips;

namespace Dsmviz.Interfaces.ViewModel.Matrix
{
    public interface IMatrixColumnHeaderViewModel : INotifyPropertyChanged
    {
        void HoverColumn(int? column);
        int? HoveredColumn { get; }

        void SelectColumn(int? column);
        int? SelectedColumn { get; }

        int ColumnCount { get; }

        int GetColumnDepth(int column);
        string GetColumnContent(int column);

        IElementToolTipViewModel? ColumnHeaderToolTipViewModel { get; }
    }
}
