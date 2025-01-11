using Dsmviz.Viewer.ViewModel.Lists.Element;
using System.Windows;
using Dsmviz.Interfaces.ViewModel.Lists.Element;

namespace Dsmviz.Viewer.View.Lists
{
    /// <summary>
    /// Interaction logic for ElementListView.xaml
    /// </summary>
    public partial class ElementListView
    {
        private IElementListViewModel? _viewModel;

        public ElementListView()
        {
            InitializeComponent();
        }

        private void ElementListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as IElementListViewModel;
        }
    }
}
