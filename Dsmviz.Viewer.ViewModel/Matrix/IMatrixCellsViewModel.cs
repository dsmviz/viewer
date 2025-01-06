using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Viewer.ViewModel.Common;
using System.ComponentModel;

namespace Dsmviz.Viewer.ViewModel.Matrix
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

        CellToolTipViewModel? CellToolTipViewModel { get; }

        int GetCellDepth(int row, int column);
        int GetCellWeight(int row, int column);
        Cycle GetCellCycle(int row, int column);

        ViewPerspective SelectedViewPerspective { get; }
    }
}
