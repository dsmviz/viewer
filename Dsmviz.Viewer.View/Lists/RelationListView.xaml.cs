using Dsmviz.Viewer.View.Editing;
using Dsmviz.Viewer.ViewModel.Editing.Relation;
using Dsmviz.Viewer.ViewModel.Lists.Relation;
using System.Windows;
using Dsmviz.Interfaces.ViewModel.Editing.Relation;
using Dsmviz.Interfaces.ViewModel.Lists.Relation;

namespace Dsmviz.Viewer.View.Lists
{
    /// <summary>
    /// Interaction logic for RelationListView.xaml
    /// </summary>
    public partial class RelationListView
    {
        private IRelationListViewModel? _viewModel;

        public RelationListView()
        {
            InitializeComponent();
        }

        private void RelationListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as IRelationListViewModel;
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
