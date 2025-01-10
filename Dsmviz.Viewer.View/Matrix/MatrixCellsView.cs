using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Viewer.ViewModel.Matrix;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Dsmviz.Interfaces.ViewModel.Matrix;

namespace Dsmviz.Viewer.View.Matrix
{
    public class MatrixCellsView : MatrixFrameworkElement
    {
        private IMatrixCellsViewModel? _matrixCellsViewModel;
        private readonly MatrixTheme _theme;
        private Rect _rect;
        private int? _hoveredRow;
        private int? _hoveredColumn;
        private readonly double _pitch;
        private readonly double _offset;
        private readonly double _verticalTextOffset = 16.0;

        public MatrixCellsView()
        {
            _theme = new MatrixTheme(this);
            _rect = new Rect(new Size(_theme.MatrixCellSize, _theme.MatrixCellSize));
            _hoveredRow = null;
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
                _matrixCellsViewModel = matrixViewModel.MatrixCellsViewModel;
                _matrixCellsViewModel.PropertyChanged += OnPropertyChanged;
                InvalidateVisual();
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMatrixCellsViewModel.CellToolTipViewModel))
            {
                ToolTip = _matrixCellsViewModel?.CellToolTipViewModel;
            }

            InvalidateVisual();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Do not use OnMouseEnter as this can be missed
            int row = GetHoveredRow(e.GetPosition(this));
            int column = GetHoveredColumn(e.GetPosition(this));
            if ((_hoveredRow != row) || (_hoveredColumn != column))
            {
                _hoveredRow = row;
                _hoveredColumn = column;
                _matrixCellsViewModel?.HoverCell(row, column);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _matrixCellsViewModel?.HoverCell(null, null);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            int row = GetHoveredRow(e.GetPosition(this));
            int column = GetHoveredColumn(e.GetPosition(this));
            _matrixCellsViewModel?.SelectCell(row, column);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_matrixCellsViewModel != null)
            {
                int matrixSize = _matrixCellsViewModel.MatrixSize;
                for (int row = 0; row < matrixSize; row++)
                {
                    for (int column = 0; column < matrixSize; column++)
                    {
                        _rect.X = _offset + column * _pitch;
                        _rect.Y = _offset + row * _pitch;

                        bool isHovered = (_matrixCellsViewModel.HoveredRow.HasValue && (row == _matrixCellsViewModel.HoveredRow.Value)) ||
                                         (_matrixCellsViewModel.HoveredColumn.HasValue && (column == _matrixCellsViewModel.HoveredColumn.Value));
                        bool isSelected = (_matrixCellsViewModel.SelectedRow.HasValue && (row == _matrixCellsViewModel.SelectedRow.Value)) ||
                                          (_matrixCellsViewModel.SelectedColumn.HasValue && (column == _matrixCellsViewModel.SelectedColumn.Value));
                        int cellDepth = _matrixCellsViewModel.GetCellDepth(row, column);
                        int weight = _matrixCellsViewModel.GetCellWeight(row, column);
                        Cycle cycle = _matrixCellsViewModel.GetCellCycle(row, column);

                        SolidColorBrush background = _theme.GetBackground(cellDepth, isHovered, isSelected);
                        dc.DrawRectangle(background, null, _rect);

                        SolidColorBrush? overlayColor = _theme.GetCellStateOverlayColor(cycle, _matrixCellsViewModel.SelectedViewPerspective);

                        if (overlayColor != null)
                        {
                            dc.DrawRectangle(overlayColor, null, _rect);
                        }

                        if (weight > 0)
                        {
                            char infinity = '\u221E';
                            string content = weight > 9999 ? infinity.ToString() : weight.ToString();

                            double textWidth = MeasureText(content);

                            Point location = new Point
                            {
                                X = (column * _pitch) + (_pitch - textWidth) / 2,
                                Y = (row * _pitch) + _verticalTextOffset
                            };
                            DrawText(dc, content, location, _theme.TextColor, _rect.Width - _theme.SpacingWidth);
                        }
                    }
                }
                Height = Width = _pitch * matrixSize;
            }
        }

        private int GetHoveredRow(Point location)
        {
            double row = (location.Y - _offset) / _pitch;
            return (int)row;
        }

        private int GetHoveredColumn(Point location)
        {
            double column = (location.X - _offset) / _pitch;
            return (int)column;
        }
    }
}
