using Dsmviz.Viewer.ViewModel.Matrix;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Dsmviz.Interfaces.ViewModel.Matrix;

namespace Dsmviz.Viewer.View.Matrix
{
    public class MatrixColumnHeaderView : MatrixFrameworkElement
    {
        private IMatrixColumnHeaderViewModel? _matrixColumnHeaderViewModel;
        private readonly MatrixTheme _theme;
        private Rect _rect;
        private int? _hoveredColumn;
        private readonly double _pitch;
        private readonly double _offset;

        public MatrixColumnHeaderView()
        {
            _theme = new MatrixTheme(this);
            _rect = new Rect(new Size(_theme.MatrixCellSize, _theme.MatrixHeaderHeight));
            _hoveredColumn = null;
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
                _matrixColumnHeaderViewModel = matrixViewModel.MatrixColumnHeaderViewModel;
                _matrixColumnHeaderViewModel.PropertyChanged += OnPropertyChanged;
                InvalidateVisual();
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMatrixColumnHeaderViewModel.ColumnHeaderToolTipViewModel))
            {
                ToolTip = _matrixColumnHeaderViewModel?.ColumnHeaderToolTipViewModel;
            }

            InvalidateVisual();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Do not use OnMouseEnter as this can be missed
            int column = GetHoveredColumn(e.GetPosition(this));
            if (_hoveredColumn != column)
            {
                _hoveredColumn = column;
                _matrixColumnHeaderViewModel?.HoverColumn(column);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _matrixColumnHeaderViewModel?.HoverColumn(null);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            int column = GetHoveredColumn(e.GetPosition(this));
            _matrixColumnHeaderViewModel?.SelectColumn(column);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_matrixColumnHeaderViewModel != null)
            {
                int matrixSize = _matrixColumnHeaderViewModel.ColumnCount;
                for (int column = 0; column < matrixSize; column++)
                {
                    _rect.X = _offset + column * _pitch;
                    _rect.Y = 0;

                    bool isHovered = _matrixColumnHeaderViewModel.HoveredColumn.HasValue && (column == _matrixColumnHeaderViewModel.HoveredColumn.Value);
                    bool isSelected = _matrixColumnHeaderViewModel.SelectedColumn.HasValue && (column == _matrixColumnHeaderViewModel.SelectedColumn.Value);
                    int columnDepth = _matrixColumnHeaderViewModel.GetColumnDepth(column);
                    SolidColorBrush background = _theme.GetBackground(columnDepth, isHovered, isSelected);

                    dc.DrawRectangle(background, null, _rect);

                    string content = _matrixColumnHeaderViewModel.GetColumnContent(column);

                    double textWidth = MeasureText(content);

                    Point location = new Point(_rect.X + 10.0, _rect.Y - _rect.Height + textWidth + 10.0);
                    DrawRotatedText(dc, content, location, _theme.TextColor, _theme.MatrixHeaderHeight - _theme.SpacingWidth);
                }

                Height = _theme.MatrixHeaderHeight + _theme.SpacingWidth;
                Width = _theme.MatrixCellSize * matrixSize + _theme.SpacingWidth;
            }
        }

        private int GetHoveredColumn(Point location)
        {
            double column = (location.X - _offset) / _pitch;
            return (int)column;
        }
    }

}
