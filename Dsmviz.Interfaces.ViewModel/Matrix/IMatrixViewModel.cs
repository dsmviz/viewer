using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Main;
using Dsmviz.Interfaces.ViewModel.Sidebar;

namespace Dsmviz.Interfaces.ViewModel.Matrix
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

        IModelSideBarViewModel ModelInfoViewModel { get; }
        IMatrixElementSideBarViewModel ElementInfoViewModel { get; }
        IMatrixCellSideBarViewModel RelationInfoViewModel { get; }

        IMatrixMetricsSelectorViewModel MatrixMetricsSelectorViewModel { get; }
        IMatrixRowMetricsViewModel MatrixRowMetricsViewModel { get; }

        IMatrixRowHeaderViewModel MatrixRowHeaderViewModel { get; }
        IMatrixColumnHeaderViewModel MatrixColumnHeaderViewModel { get; }
        IMatrixCellsViewModel MatrixCellsViewModel { get; }

        public ViewPerspective SelectedViewPerspective { get; }

        bool IsSearchActive { get; }

        double ZoomLevel { get; set; }
    }
}
