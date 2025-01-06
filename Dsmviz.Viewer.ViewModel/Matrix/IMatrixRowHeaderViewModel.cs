using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public interface IMatrixRowHeaderViewModel : INotifyPropertyChanged
    {
        void ContentChanged();

        ObservableCollection<MatrixRowHeaderTreeItemViewModel> ElementViewModelTree { get; }

        void HoverTreeItem(MatrixRowHeaderTreeItemViewModel hoveredTreeItem);
        MatrixRowHeaderTreeItemViewModel? HoveredTreeItem { get; }

        void SelectTreeItem(MatrixRowHeaderTreeItemViewModel selectedTreeItem);
        MatrixRowHeaderTreeItemViewModel? SelectedTreeItem { get; }
    }
}
