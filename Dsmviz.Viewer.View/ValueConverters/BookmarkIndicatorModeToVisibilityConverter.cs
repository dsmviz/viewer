using Dsmviz.Viewer.ViewModel.Common;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Dsmviz.ViewModel.Interfaces.Common;

namespace Dsmviz.Viewer.View.ValueConverters
{
    public class BookmarkIndicatorModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ViewPerspective viewPerspective = (ViewPerspective)value;
            return (viewPerspective == ViewPerspective.Bookmarks) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
