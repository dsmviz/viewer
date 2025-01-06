using Dsmviz.Viewer.View.Lists;
using Dsmviz.Viewer.ViewModel.Lists.Element;
using Dsmviz.Viewer.ViewModel.Lists.Relation;
using Dsmviz.Viewer.ViewModel.SideBar;
using System.Windows;

namespace Dsmviz.Viewer.View.SideBar
{
    /// <summary>
    /// Interaction logic for MatrixElementInfoView.xaml
    /// </summary>
    public partial class SelectedMatrixElementSideBarView
    {
        private SelectedMatrixElementSideBarViewModel? _viewModel;

        public SelectedMatrixElementSideBarView()
        {
            InitializeComponent();
        }

        private void OnElementsReportReady(object sender, ElementListViewModel e)
        {
            ElementListView view = new ElementListView
            {
                DataContext = e,
            };
            view.Show();
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
            _viewModel = DataContext as SelectedMatrixElementSideBarViewModel;
            if (_viewModel != null)
            {
                _viewModel.ElementsReportReady += OnElementsReportReady;
                _viewModel.RelationsReportReady += OnRelationsReportReady;
            }
            InvalidateVisual();
        }
    }
}
