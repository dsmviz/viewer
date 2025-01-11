using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Matrix;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Matrix;

namespace Dsmviz.Viewer.View.Matrix
{
    public class MatrixRowHeaderTreeItemView : MatrixFrameworkElement
    {
        private readonly IMatrixRowHeaderViewModel _matrixRowHeaderViewModel;
        private static readonly string DataObjectName = "Element";
        private readonly MatrixTheme _theme;
        private IMatrixRowHeaderTreeItemViewModel? _rowHeaderItemViewModel;
        private readonly int _indicatorWith = 5;
        private bool _isValidDropTarget;
        private bool _isInvalidDropTarget;
        private bool _isHovered;

        public MatrixRowHeaderTreeItemView(IMatrixRowHeaderViewModel matrixRowHeaderViewModel, MatrixTheme theme)
        {
            _matrixRowHeaderViewModel = matrixRowHeaderViewModel;
            _theme = theme;

            AllowDrop = true;

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _rowHeaderItemViewModel = e.NewValue as IMatrixRowHeaderTreeItemViewModel;
            if (_rowHeaderItemViewModel != null)
            {
                _rowHeaderItemViewModel.PropertyChanged += OnPropertyChanged;
                ToolTip = _rowHeaderItemViewModel.ToolTipViewModel;
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Do not use OnMouseEnter as this can be missed
            if (_rowHeaderItemViewModel != null && !_isHovered)
            {
                _matrixRowHeaderViewModel.HoverTreeItem(_rowHeaderItemViewModel);
                _isHovered = true;
            }

            if (_rowHeaderItemViewModel != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject data = new DataObject();
                    data.SetData(DataObjectName, _rowHeaderItemViewModel);
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (_rowHeaderItemViewModel != null)
            {
                // Check if mouse above arrow. Also check for X and Y larger than zero
                // to ensure clicking on works when content has been rendered.
                // When content is not rendered yet GetPosition returns 0,0.
                Point pt = e.GetPosition(this);
                if (pt is { X: > 0, X: < 20, Y: > 0, Y: < 20 }) // Only expand when arrow area clicked
                {
                    _rowHeaderItemViewModel.ToggleElementExpanded();
                    _matrixRowHeaderViewModel.ContentChanged();
                }
                else
                {
                    // 
                }

                _matrixRowHeaderViewModel.SelectTreeItem(_rowHeaderItemViewModel);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (_rowHeaderItemViewModel != null && _isHovered)
            {
                _matrixRowHeaderViewModel.HoverTreeItem(null);
                _isHovered = false;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);

            Mouse.SetCursor(e.Effects.HasFlag(DragDropEffects.Move) ? Cursors.Pen : Cursors.Arrow);
            e.Handled = true;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            _isValidDropTarget = false;
            _isInvalidDropTarget = false;

            if (IsValidDropTarget(e))
            {
                _isValidDropTarget = true;
            }
            else
            {
                _isInvalidDropTarget = true;
            }
            InvalidateVisual();
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            _isValidDropTarget = false;
            _isInvalidDropTarget = false;
            InvalidateVisual();
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            e.Effects = IsValidDropTarget(e) ? DragDropEffects.Move : DragDropEffects.None;

            e.Handled = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataObjectName))
            {
                IMatrixRowHeaderTreeItemViewModel? dragged = (IMatrixRowHeaderTreeItemViewModel)e.Data.GetData(DataObjectName);
                IMatrixRowHeaderTreeItemViewModel? dropTarget = _rowHeaderItemViewModel;

                if ((dragged != null) &&
                    (dropTarget != null) &&
                    (dragged != dropTarget))
                {
                    int index = GetDropAtIndex(e);
                    Tuple<IElement, IElement, int> moveParameter = new Tuple<IElement, IElement, int>(dragged.Element, dropTarget.Element, index);
                    // TODO FIX
                    //_rowHeaderItemViewModel.ChangeElementParentCommand.Execute(moveParameter);
                }

                e.Effects = DragDropEffects.Move;
            }
            _isValidDropTarget = false;
            e.Handled = true;
            InvalidateVisual();
        }

        private bool IsValidDropTarget(DragEventArgs e)
        {
            bool isValidDropTarget = false;

            if (e.Data.GetDataPresent(DataObjectName))
            {
                IMatrixRowHeaderTreeItemViewModel? dragged = (IMatrixRowHeaderTreeItemViewModel)e.Data.GetData(DataObjectName);
                IMatrixRowHeaderTreeItemViewModel? dropTarget = _rowHeaderItemViewModel;

                if ((dragged != null) &&
                    (dropTarget != null) &&
                    (!dropTarget.Element.IsRecursiveChildOf(dragged.Element)))
                {
                    isValidDropTarget = true;
                }
            }

            return isValidDropTarget;
        }

        private int GetDropAtIndex(DragEventArgs e)
        {
            Point point = e.GetPosition(this);

            double pitch = _theme.MatrixCellSize + 2.0;

            int index = (int)(point.Y / pitch);
            return index;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if ((_rowHeaderItemViewModel != null) && (ActualWidth > _theme.SpacingWidth) && (ActualHeight > _theme.SpacingWidth))
            {
                bool isHovered = _matrixRowHeaderViewModel.HoveredTreeItem == _rowHeaderItemViewModel;
                bool isSelected = _matrixRowHeaderViewModel.SelectedTreeItem == _rowHeaderItemViewModel;
                SolidColorBrush background;
                if (_isValidDropTarget)
                {
                    background = _theme.MatrixColorWarning;
                }
                else if (_isInvalidDropTarget)
                {
                    background = _theme.MatrixColorError;
                }
                else
                {
                    background = _theme.GetBackground(_rowHeaderItemViewModel.Depth, isHovered, isSelected);
                }
                Rect backgroundRect = new Rect(1.0, 1.0, ActualWidth - _theme.SpacingWidth, ActualHeight - _theme.SpacingWidth);
                dc.DrawRectangle(background, null, backgroundRect);

                string content = _rowHeaderItemViewModel.Name;

                if (_rowHeaderItemViewModel.IsExpanded)
                {
                    Point textLocation = new Point(backgroundRect.X + 10.0, backgroundRect.Y - 20.0);
                    DrawRotatedText(dc, content, textLocation, _theme.TextColor, backgroundRect.Height - 20.0);
                }
                else
                {
                    Rect indicatorRect = new Rect(backgroundRect.Width - _indicatorWith, 1.0, _indicatorWith, ActualHeight - _theme.SpacingWidth);

                    if (_rowHeaderItemViewModel.IsSearchActive)
                    {
                        if (_rowHeaderItemViewModel.IsMatch)
                        {
                            dc.DrawRectangle(_theme.MatrixColorMatch, null, indicatorRect);
                        }
                    }
                    else
                    {
                        switch (_rowHeaderItemViewModel.SelectedViewPerspective)
                        {
                            case ViewPerspective.Explore:
                                {
                                    SolidColorBrush? brush = GetIndicatorColor();
                                    if (brush != null)
                                    {
                                        dc.DrawRectangle(brush, null, indicatorRect);
                                    }
                                }
                                break;
                            case ViewPerspective.Bookmarks:
                                {
                                    if (_rowHeaderItemViewModel.IsBookmarked)
                                    {
                                        dc.DrawRectangle(_theme.MatrixColorBookmark, null, indicatorRect);
                                    }
                                }
                                break;
                        }
                    }

                    if (ActualWidth > 70.0)
                    {
                        Point contentTextLocation = new Point(backgroundRect.X + 20.0, backgroundRect.Y + 15.0);
                        DrawText(dc, content, contentTextLocation, _theme.TextColor, ActualWidth - 70.0);
                    }

                    string order = _rowHeaderItemViewModel.Order.ToString();
                    double textWidth = MeasureText(order);

                    Point orderTextLocation = new Point(backgroundRect.X - 25.0 + backgroundRect.Width - textWidth, backgroundRect.Y + 15.0);
                    if (orderTextLocation.X > 0)
                    {
                        DrawText(dc, order, orderTextLocation, _theme.TextColor, ActualWidth - 25.0);
                    }
                }

                Point expanderLocation = new Point(backgroundRect.X + 1.0, backgroundRect.Y + 1.0);
                DrawExpander(dc, expanderLocation);
            }
        }

        private SolidColorBrush? GetIndicatorColor()
        {
            SolidColorBrush? brush = null;
            if (_rowHeaderItemViewModel != null)
            {
                if (_rowHeaderItemViewModel.IsConsumer)
                {
                    // Cyclic
                    brush = _rowHeaderItemViewModel.IsProvider ? _theme.MatrixColorWarning : _theme.MatrixColorConsumer;
                }
                else if (_rowHeaderItemViewModel.IsProvider)
                {
                    brush = _theme.MatrixColorProvider;
                }
            }

            return brush;
        }

        private void DrawExpander(DrawingContext dc, Point location)
        {
            if (_rowHeaderItemViewModel.IsExpandable)
            {
                if (_isHovered)
                {
                    dc.DrawText(
                        _rowHeaderItemViewModel.IsExpanded
                            ? _theme.DownArrowFormattedHoverText
                            : _theme.RightArrowFormattedHoverText, location);

                }
                else
                {
                    dc.DrawText(
                        _rowHeaderItemViewModel.IsExpanded
                            ? _theme.DownArrowFormattedText
                            : _theme.RightArrowFormattedText, location);
                }
            }
        }
    }
}