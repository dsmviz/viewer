using Dsmviz.Viewer.ViewModel.Matrix;
using System.Windows;
using System.Windows.Controls;
using Dsmviz.Interfaces.ViewModel.Matrix;

namespace Dsmviz.Viewer.View.Matrix
{
    public class MatrixRowHeaderView : Canvas
    {
        private IMatrixRowHeaderViewModel? _matrixRowHeaderViewModel;
        private readonly MatrixTheme _theme;

        public MatrixRowHeaderView()
        {
            _theme = new MatrixTheme(this);

            DataContextChanged += OnDataContextChanged;
            SizeChanged += OnSizeChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is IMatrixViewModel matrixViewModel)
            {
                _matrixRowHeaderViewModel = matrixViewModel.MatrixRowHeaderViewModel;
                _matrixRowHeaderViewModel.PropertyChanged += OnPropertyChanged;
                InvalidateVisual();
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CreateChildViews();
            RedrawChildViews();
        }

        protected void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateChildViews();
            RedrawChildViews();
        }

        private void RedrawChildViews()
        {
            foreach (var child in Children)
            {
                MatrixRowHeaderTreeItemView renderedRowHeaderItemView = child as MatrixRowHeaderTreeItemView;
                renderedRowHeaderItemView?.InvalidateVisual();
            }
        }

        private void CreateChildViews()
        {
            double y = 0.0;

            Children.Clear();
            if (_matrixRowHeaderViewModel?.ElementViewModelTree != null)
            {
                foreach (IMatrixRowHeaderTreeItemViewModel elementViewModel in _matrixRowHeaderViewModel.ElementViewModelTree)
                {
                    Rect rect = GetCalculatedSize(elementViewModel, y);

                    MatrixRowHeaderTreeItemView elementView = new MatrixRowHeaderTreeItemView(_matrixRowHeaderViewModel, _theme)
                    {
                        Height = rect.Height,
                        Width = rect.Width
                    };
                    SetTop(elementView, rect.Top);
                    SetLeft(elementView, rect.Left);
                    elementView.DataContext = elementViewModel;
                    Children.Add(elementView);

                    CreateChildViews(elementViewModel, y);

                    y += rect.Height;
                }
            }

            Height = y;
            //Width = 5000; // Should be enough to draw very deep tree
        }

        private void CreateChildViews(IMatrixRowHeaderTreeItemViewModel elementViewModel, double y)
        {
            foreach (IMatrixRowHeaderTreeItemViewModel child in elementViewModel.Children)
            {
                Rect rect = GetCalculatedSize(child, y);

                MatrixRowHeaderTreeItemView elementView = new MatrixRowHeaderTreeItemView(_matrixRowHeaderViewModel, _theme)
                {
                    Height = rect.Height,
                    Width = rect.Width
                };
                SetTop(elementView, rect.Top);
                SetLeft(elementView, rect.Left);
                elementView.DataContext = child;
                Children.Add(elementView);

                CreateChildViews(child, y);

                y += rect.Height;
            }
        }

        private Rect GetCalculatedSize(IMatrixRowHeaderTreeItemViewModel viewModel, double y)
        {
            Rect rect;

            int leafElementCount = viewModel.LeafElementCount;

            double pitch = _theme.MatrixCellSize + 2.0;
            if (viewModel.IsExpanded)
            {
                double x = viewModel.Depth * 26.0;
                double width = pitch;
                if (width > ActualWidth)
                {
                    width = ActualWidth;
                }
                double height = leafElementCount * pitch;
                rect = new Rect(x, y, width, height);
            }
            else
            {
                double x = viewModel.Depth * 26.0;
                double width = ActualWidth - x + 1.0;
                if (width < 0)
                {
                    width = 0;
                }
                double height = pitch;
                rect = new Rect(x, y, width, height);
            }

            return rect;
        }
    }
}

