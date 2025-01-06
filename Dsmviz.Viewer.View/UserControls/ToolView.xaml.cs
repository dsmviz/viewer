using Dsmviz.Viewer.ViewModel.Main;
using System.Windows;
using System.Windows.Controls;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Dsmviz.Viewer.View.UserControls
{
    /// <summary>
    /// Interaction logic for ControlAndInfoView.xaml
    /// </summary>
    public partial class ToolView
    {
        private MainViewModel? _mainViewModel;

        public ToolView()
        {
            InitializeComponent();
        }

        private void ToolView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel = DataContext as MainViewModel;
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            string filename = GetSelectedFile();
            if (_mainViewModel.OpenFileCommand.CanExecute(filename))
            {
                _mainViewModel.OpenFileCommand.Execute(filename);
            }
        }

        private string GetSelectedFile()
        {
            string selectedFile = null;

            OpenFileDialog dlg = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "dsm",
                Filter = "DSM model|*.dsm|DSM input|*.dsi|All Types|*.dsm;*.dsi;*.dsr;*.dsa",
                Title = "Open DSM project"
            };

            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                selectedFile = dlg.FileName;
            }

            return selectedFile;
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            if (toolBar.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }
    }
}
