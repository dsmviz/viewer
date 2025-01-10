using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Viewer.ViewModel.Common;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Dsmviz.ViewModel.Interfaces.Common;

namespace Dsmviz.Viewer.View.Matrix
{
    public class MatrixTheme
    {
        private SolidColorBrush[] _brushes = [];
        private readonly FrameworkElement _frameworkElement;

        private readonly double _blendHighlight1 = 0.75;
        private readonly double _blendHighlight2 = 0.55;
        private readonly double _blendAccent = 0.25;

        public MatrixTheme(FrameworkElement frameworkElement)
        {
            _frameworkElement = frameworkElement;

            MatrixCellSize = (double)_frameworkElement.FindResource("MatrixCellSize");
            MatrixHeaderHeight = (double)_frameworkElement.FindResource("MatrixHeaderHeight");
            MatrixMetricsViewWidth = (double)_frameworkElement.FindResource("MatrixMetricsViewWidth");
            TextColor = (SolidColorBrush)_frameworkElement.FindResource("TextColor");
            MatrixColorConsumer = GetBrush("MatrixColorConsumer", _blendHighlight1);
            MatrixColorProvider = GetBrush("MatrixColorProvider", _blendHighlight1);
            MatrixColorMatch = GetBrush("MatrixColorMatch", _blendHighlight1);
            MatrixColorBookmark = GetBrush("MatrixColorBookmark", _blendHighlight1);
            MatrixColorWarning = GetBrush("MatrixColorViolation", _blendHighlight1);
            MatrixColorError = GetBrush("MatrixColorRuleForbidden", _blendHighlight1);

            MatrixColorSystemCycle = GetBrush("MatrixColorViolation", _blendHighlight1);

            MatrixColorHierarchicalCycle = GetBrush("MatrixColorViolation", _blendHighlight1);
            MatrixColorHierarchicalCycleContributor = GetBrush("MatrixColorViolation", _blendHighlight1);

            MatrixColorRequiredRuleAccent = GetBrush("MatrixColorRuleRequired", _blendAccent);
            MatrixColorRequiredRuleHighlight = GetBrush("MatrixColorRuleRequired", _blendHighlight1);

            MatrixColorAllowedRuleAccent = GetBrush("MatrixColorRuleAllowed", _blendAccent);
            MatrixColorAllowedRuleHighlight = GetBrush("MatrixColorRuleAllowed", _blendHighlight1);

            MatrixColorExceptionRuleAccent = GetBrush("MatrixColorRuleException", _blendAccent);
            MatrixColorExceptionRuleHighlight = GetBrush("MatrixColorRuleException", _blendHighlight1);

            MatrixColorForbiddenRuleAccent = GetBrush("MatrixColorRuleForbidden", _blendAccent);
            MatrixColorForbiddenRuleHighlight = GetBrush("MatrixColorRuleForbidden", _blendHighlight1);
            MatrixColorForbiddenRuleViolationContributor = GetBrush("MatrixColorRuleForbidden", _blendHighlight2);

            LeftArrow = (string)_frameworkElement.FindResource("LeftArrowIcon");
            RightArrow = (string)_frameworkElement.FindResource("RightArrowIcon");
            UpArrow = (string)_frameworkElement.FindResource("UpArrowIcon");
            DownArrow = (string)_frameworkElement.FindResource("DownArrowIcon");

            MatrixColorRequiredRuleAccent.Opacity = 0.25;
            MatrixColorAllowedRuleAccent.Opacity = 0.25;
            MatrixColorExceptionRuleAccent.Opacity = 0.25;
            MatrixColorForbiddenRuleAccent.Opacity = 0.25;

            RightArrowFormattedText = new FormattedText(RightArrow,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                10,
                TextColor);

            DownArrowFormattedText = new FormattedText(DownArrow,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                10,
                TextColor);

            RightArrowFormattedHoverText = new FormattedText(RightArrow,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                TextColor);

            DownArrowFormattedHoverText = new FormattedText(DownArrow,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                TextColor);
        }

        public double SpacingWidth => 2.0;
        public double MatrixCellSize { get; }
        public double MatrixHeaderHeight { get; }
        public double MatrixMetricsViewWidth { get; }
        public SolidColorBrush TextColor { get; }
        public SolidColorBrush MatrixColorConsumer { get; }
        public SolidColorBrush MatrixColorProvider { get; }
        public SolidColorBrush MatrixColorMatch { get; }
        public SolidColorBrush MatrixColorBookmark { get; }
        public SolidColorBrush MatrixColorWarning { get; }
        public SolidColorBrush MatrixColorError { get; }
        public SolidColorBrush MatrixColorSystemCycle { get; }
        public SolidColorBrush MatrixColorHierarchicalCycle { get; }
        public SolidColorBrush MatrixColorHierarchicalCycleContributor { get; }
        public SolidColorBrush MatrixColorRequiredRuleAccent { get; }
        public SolidColorBrush MatrixColorRequiredRuleHighlight { get; }
        public SolidColorBrush MatrixColorAllowedRuleAccent { get; }
        public SolidColorBrush MatrixColorAllowedRuleHighlight { get; }
        public SolidColorBrush MatrixColorExceptionRuleAccent { get; }
        public SolidColorBrush MatrixColorExceptionRuleHighlight { get; }
        public SolidColorBrush MatrixColorForbiddenRuleAccent { get; }
        public SolidColorBrush MatrixColorForbiddenRuleHighlight { get; }
        public SolidColorBrush MatrixColorForbiddenRuleViolationContributor { get; }
        public string LeftArrow { get; }
        public string RightArrow { get; }
        public string UpArrow { get; }
        public string DownArrow { get; }
        public FormattedText RightArrowFormattedText { get; }
        public FormattedText DownArrowFormattedText { get; }
        public FormattedText RightArrowFormattedHoverText { get; }
        public FormattedText DownArrowFormattedHoverText { get; }

        public SolidColorBrush GetBackground(int depth, bool isHovered, bool isSelected)
        {
            UpdateBrushes();

            int colorIndex = (depth % 4) * 4 + 4;

            if (isHovered)
            {
                colorIndex += 1;
            }

            if (isSelected)
            {
                colorIndex += 2;
            }

            return _brushes[colorIndex];
        }

        public SolidColorBrush? GetCellStateOverlayColor(Cycle cycle, ViewPerspective viewPerspective)
        {
            if (viewPerspective == ViewPerspective.Explore)
            {
                switch (cycle)
                {
                    case Cycle.System:
                        return MatrixColorSystemCycle;
                    case Cycle.Hierarchical:
                        return MatrixColorHierarchicalCycle;
                    case Cycle.HierarchicalContributor:
                        return MatrixColorHierarchicalCycleContributor;
                    case Cycle.None:
                        return null;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        private SolidColorBrush GetBrush(string resource, double opacity)
        {
            SolidColorBrush brush = (SolidColorBrush)_frameworkElement.FindResource(resource);
            SolidColorBrush clonedBrush = brush.Clone();
            clonedBrush.Opacity = opacity;
            return clonedBrush;
        }
        private void UpdateBrushes()
        {
            _brushes = new SolidColorBrush[20];

            SolidColorBrush brushBackground = (SolidColorBrush)_frameworkElement.FindResource("MatrixColorBackground");
            SolidColorBrush brush1 = (SolidColorBrush)_frameworkElement.FindResource("MatrixColor1");
            SolidColorBrush brush2 = (SolidColorBrush)_frameworkElement.FindResource("MatrixColor2");
            SolidColorBrush brush3 = (SolidColorBrush)_frameworkElement.FindResource("MatrixColor3");
            SolidColorBrush brush4 = (SolidColorBrush)_frameworkElement.FindResource("MatrixColor4");
            double highlightFactorHovered = (double)_frameworkElement.FindResource("HighlightFactorHovered");
            double highlightFactorSelected = (double)_frameworkElement.FindResource("HighlightFactorSelected");

            SetBrush(0, brushBackground, highlightFactorHovered, highlightFactorSelected);
            SetBrush(1, brush1, highlightFactorHovered, highlightFactorSelected);
            SetBrush(2, brush2, highlightFactorHovered, highlightFactorSelected);
            SetBrush(3, brush3, highlightFactorHovered, highlightFactorSelected);
            SetBrush(4, brush4, highlightFactorHovered, highlightFactorSelected);
        }

        private void SetBrush(int colorIndex, SolidColorBrush brush, double highlightFactorHovered, double highlightFactorSelected)
        {
            int index = colorIndex * 4;
            _brushes[index] = brush;
            _brushes[index + 1] = GetHighlightBrush(brush, highlightFactorHovered);
            _brushes[index + 2] = GetHighlightBrush(brush, highlightFactorSelected);
            _brushes[index + 3] = GetHighlightBrush(brush, highlightFactorHovered * highlightFactorSelected);
        }

        public static SolidColorBrush GetHighlightBrush(SolidColorBrush color, double multiplicationFactor)
        {
            float factor = (float)multiplicationFactor;
            return new SolidColorBrush(Color.Multiply(color.Color, factor));
        }
    }
}
