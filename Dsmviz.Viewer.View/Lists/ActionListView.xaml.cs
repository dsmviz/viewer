using Dsmviz.Viewer.ViewModel.Lists.Action;
using System.Windows;
using Dsmviz.Interfaces.ViewModel.Lists.Action;

namespace Dsmviz.Viewer.View.Lists
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class ActionListView
    {
        private IActionListViewModel? _viewModel;

        public ActionListView()
        {
            InitializeComponent();
        }

        private void ActionListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as IActionListViewModel;
        }
    }
}
