using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Dsmviz.Interfaces.ViewModel.Matrix
{
    public interface IMatrixRowHeaderViewModel : INotifyPropertyChanged
    {
        void ContentChanged();

        ObservableCollection<IMatrixRowHeaderTreeItemViewModel> ElementViewModelTree { get; }

        void HoverTreeItem(IMatrixRowHeaderTreeItemViewModel hoveredTreeItem);
        IMatrixRowHeaderTreeItemViewModel? HoveredTreeItem { get; }

        void SelectTreeItem(IMatrixRowHeaderTreeItemViewModel selectedTreeItem);
        IMatrixRowHeaderTreeItemViewModel? SelectedTreeItem { get; }
    }
}
