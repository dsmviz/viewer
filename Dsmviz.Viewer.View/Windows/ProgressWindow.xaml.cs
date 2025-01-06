using System.Windows;

namespace Dsmviz.Viewer.View.Windows
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
