using Dsmviz.Viewer.View.Lists;
using Dsmviz.Viewer.ViewModel.Lists.Relation;
using Dsmviz.Viewer.ViewModel.SideBar;
using System.Windows;
using Dsmviz.Interfaces.ViewModel.Lists.Relation;
using Dsmviz.Interfaces.ViewModel.Sidebar;

namespace Dsmviz.Viewer.View.SideBar
{
    /// <summary>
    /// Interaction logic for CellInfoSideBarView.xaml
    /// </summary>
    public partial class SelectedMatrixCellSideBarView
    {
        private IMatrixCellSideBarViewModel? _viewModel;

        public SelectedMatrixCellSideBarView()
        {
            InitializeComponent();
        }

        private void OnRelationsReportReady(object sender, IRelationListViewModel e)
        {
            RelationListView view = new RelationListView
            {
                DataContext = e,
            };
            view.Show();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as IMatrixCellSideBarViewModel;
            if (_viewModel != null)
            {
                _viewModel.RelationsReportReady += OnRelationsReportReady;
            }
            InvalidateVisual();
        }
    }
}
