using Dsmviz.Viewer.View.Lists;
using Dsmviz.Viewer.ViewModel.Lists.Relation;
using Dsmviz.Viewer.ViewModel.SideBar;
using System.Windows;

namespace Dsmviz.Viewer.View.SideBar
{
    /// <summary>
    /// Interaction logic for CellInfoSideBarView.xaml
    /// </summary>
    public partial class SelectedMatrixCellSideBarView
    {
        private SelectedMatrixCellSideBarViewModel? _viewModel;

        public SelectedMatrixCellSideBarView()
        {
            InitializeComponent();
        }

        private void OnRelationsReportReady(object sender, RelationListViewModel e)
        {
            RelationListView view = new RelationListView
            {
                DataContext = e,
            };
            view.Show();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as SelectedMatrixCellSideBarViewModel;
            if (_viewModel != null)
            {
                _viewModel.RelationsReportReady += OnRelationsReportReady;
            }
            InvalidateVisual();
        }
    }
}
