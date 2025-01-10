using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.ViewModel.Interfaces.Common;

namespace Dsmviz.ViewModel.Interfaces.Matrix
{
    public interface IMatrixViewModel
    {
        void ContentChanged();

        void HoverRow(int? row);
        void HoverColumn(int? column);
        void HoverCell(int? row, int? column);
        int? HoveredRow { get; }
        int? HoveredColumn { get; }

        void SelectRow(int? row);
        void SelectColumn(int? column);
        void SelectCell(int? row, int? column);
        int? SelectedRow { get; }
        int? SelectedColumn { get; }

        IElement? SelectedConsumer { get; }
        IElement? SelectedProvider { get; }

        ViewPerspective SelectedViewPerspective { get; }
        bool IsSearchActive { get; }

        double ZoomLevel { get; set; }
    }
}
