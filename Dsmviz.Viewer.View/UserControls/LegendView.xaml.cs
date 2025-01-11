using Dsmviz.Viewer.ViewModel.Main;
using System.Windows;
using Dsmviz.Interfaces.ViewModel.Main;

namespace Dsmviz.Viewer.View.UserControls
{
    /// <summary>
    /// Interaction logic for LegendView.xaml
    /// </summary>
    public partial class LegendView
    {
        private IMainViewModel? _mainViewModel;

        public LegendView()
        {
            InitializeComponent();
        }

        private void LegendView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel = DataContext as IMainViewModel;
        }
    }
}
