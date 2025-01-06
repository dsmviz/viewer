using System.Windows;

namespace Dsmviz.Viewer.View.Matrix
{
    /// <summary>
    /// Interaction logic for MatrixTopCornerView.xaml
    /// </summary>
    public partial class MatrixTopCornerView
    {
        public MatrixTopCornerView()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
