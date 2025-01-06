using Dsmviz.Viewer.ViewModel.Matrix;
using System.Windows;

namespace Dsmviz.Viewer.View.Matrix
{
    /// <summary>
    /// Interaction logic for MatrixMetricsSelectorView.xaml
    /// </summary>
    public partial class MatrixMetricsSelectorView
    {
        private IMatrixMetricsSelectorViewModel? _matrixMetricsSelectorViewMode;

        public MatrixMetricsSelectorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MatrixViewModel matrixViewModel)
            {
                _matrixMetricsSelectorViewMode = matrixViewModel.MatrixMetricsSelectorViewModel;
                _matrixMetricsSelectorViewMode.PropertyChanged += OnPropertyChanged;
                InvalidateVisual();
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateVisual();
        }
    }
}
