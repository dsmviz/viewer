using Dsmviz.Viewer.ViewModel.Lists.Action;
using System.Windows;

namespace Dsmviz.Viewer.View.Lists
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class ActionListView
    {
        private ActionListViewModel? _viewModel;

        public ActionListView()
        {
            InitializeComponent();
        }

        private void ActionListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as ActionListViewModel;
        }
    }
}
