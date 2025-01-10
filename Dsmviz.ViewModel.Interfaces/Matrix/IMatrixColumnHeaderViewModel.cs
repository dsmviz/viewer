using System.ComponentModel;
using Dsmviz.ViewModel.Interfaces.Tooltips;

namespace Dsmviz.ViewModel.Interfaces.Matrix
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
