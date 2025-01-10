using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.ViewModel.Interfaces.Lists.Relation;

namespace Dsmviz.ViewModel.Interfaces.Sidebar
{
    public interface IMatrixCellSideBarViewModel
    {
        event EventHandler<IRelationListViewModel>? RelationsReportReady;
        bool Selected { get; }
        IElement? SelectedConsumer { get; set; }
        IElement? SelectedProvider { get; set; }
        int CellDerivedWeight { get; }
        string CellCycle { get; }
        int RelationCount { get; }
        ICommand ShowRelationsListCommand { get; }
        void SelectCell(IElement selectedConsumer, IElement selectedProvider);
        void Unselect();
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
