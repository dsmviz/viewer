using System.Windows;

namespace Dsmviz.Viewer.View.SideBar
{
    /// <summary>
    /// Interaction logic for ModelInfoSideBarView.xaml
    /// </summary>
    public partial class ModelSideBarView
    {
        public ModelSideBarView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
        }
    }
}
