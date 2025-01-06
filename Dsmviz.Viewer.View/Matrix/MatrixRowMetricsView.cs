using Dsmviz.Viewer.ViewModel.Matrix;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Dsmviz.Viewer.View.Matrix
{
    public class MatrixRowMetricsView : MatrixFrameworkElement
    {
        private IMatrixRowMetricsViewModel? _matrixRowMetricsViewModel;
        private readonly MatrixTheme _theme;
        private Rect _rect;
        private int? _hoveredRow;
        private readonly double _pitch;
        private readonly double _offset;

        public MatrixRowMetricsView()
        {
            _theme = new MatrixTheme(this);
            _rect = new Rect(new Size(_theme.MatrixMetricsViewWidth, _theme.MatrixCellSize));
            _hoveredRow = null;
            _pitch = _theme.MatrixCellSize + _theme.SpacingWidth;
            _offset = _theme.SpacingWidth / 2;

            DataContextChanged += OnDataContextChanged;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseLeave += OnMouseLeave;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MatrixViewModel matrixViewModel)
            {
                _matrixRowMetricsViewModel = matrixViewModel.MatrixRowMetricsViewModel;
                _matrixRowMetricsViewModel.PropertyChanged += OnPropertyChanged;
                InvalidateVisual();
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateVisual();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Do not use OnMouseEnter as this can be missed
            int row = GetHoveredRow(e.GetPosition(this));
            if (_hoveredRow != row)
            {
                _hoveredRow = row;
                _matrixRowMetricsViewModel?.HoverRow(row);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _matrixRowMetricsViewModel?.HoverRow(null);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            int row = GetHoveredRow(e.GetPosition(this));
            _matrixRowMetricsViewModel?.SelectRow(row);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_matrixRowMetricsViewModel != null)
            {
                int matrixSize = _matrixRowMetricsViewModel.RowCount;
                for (int row = 0; row < matrixSize; row++)
                {
                    _rect.X = 0;
                    _rect.Y = _offset + row * _pitch;

                    bool isHovered = _matrixRowMetricsViewModel.HoveredRow.HasValue && (row == _matrixRowMetricsViewModel.HoveredRow.Value);
                    bool isSelected = _matrixRowMetricsViewModel.SelectedRow.HasValue && (row == _matrixRowMetricsViewModel.SelectedRow.Value);
                    int rowDepth = _matrixRowMetricsViewModel.GetRowDepth(row);
                    SolidColorBrush background = _theme.GetBackground(rowDepth, isHovered, isSelected);

                    dc.DrawRectangle(background, null, _rect);

                    string content = _matrixRowMetricsViewModel.GetRowMetric(row);
                    double textWidth = MeasureText(content);
                    Point texLocation = new Point(_rect.X + _rect.Width - 30.0 - textWidth, _rect.Y + 15.0);
                    DrawText(dc, content, texLocation, _theme.TextColor, _rect.Width - _theme.SpacingWidth);
                }

                Height = _theme.MatrixHeaderHeight + _theme.SpacingWidth;
                Width = _theme.MatrixCellSize * matrixSize + _theme.SpacingWidth;
            }
        }

        private int GetHoveredRow(Point location)
        {
            double row = (location.Y - _offset) / _pitch;
            return (int)row;
        }
    }

}
