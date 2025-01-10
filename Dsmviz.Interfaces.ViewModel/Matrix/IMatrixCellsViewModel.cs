using System.ComponentModel;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Tooltips;

namespace Dsmviz.Interfaces.ViewModel.Matrix
{
    public interface IMatrixCellsViewModel : INotifyPropertyChanged
    {
        void HoverCell(int? row, int? column);
        int? HoveredRow { get; }
        int? HoveredColumn { get; }

        void SelectCell(int? row, int? column);
        int? SelectedRow { get; }
        int? SelectedColumn { get; }

        int MatrixSize { get; }

        ICellToolTipViewModel? CellToolTipViewModel { get; }

        int GetCellDepth(int row, int column);
        int GetCellWeight(int row, int column);
        Cycle GetCellCycle(int row, int column);

        ViewPerspective SelectedViewPerspective { get; }
    }
}
