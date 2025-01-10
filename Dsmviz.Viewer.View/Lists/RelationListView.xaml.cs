using Dsmviz.Viewer.View.Editing;
using Dsmviz.Viewer.ViewModel.Editing.Relation;
using Dsmviz.Viewer.ViewModel.Lists.Relation;
using System.Windows;
using Dsmviz.ViewModel.Interfaces.Editing.Relation;

namespace Dsmviz.Viewer.View.Lists
{
    /// <summary>
    /// Interaction logic for RelationListView.xaml
    /// </summary>
    public partial class RelationListView
    {
        private RelationListViewModel? _viewModel;

        public RelationListView()
        {
            InitializeComponent();
        }

        private void RelationListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as RelationListViewModel;
            if (_viewModel != null)
            {
                _viewModel.RelationAddStarted += OnRelationAddStarted;
                _viewModel.RelationEditStarted += OnRelationEditStarted;
            }
        }

        private void OnRelationAddStarted(object sender, IRelationEditViewModel viewModel)
        {
            RelationEditDialog view = new RelationEditDialog { DataContext = viewModel };
            view.ShowDialog();
        }

        private void OnRelationEditStarted(object sender, IRelationEditViewModel viewModel)
        {
            RelationEditDialog view = new RelationEditDialog { DataContext = viewModel };
            view.ShowDialog();
        }
    }
}
