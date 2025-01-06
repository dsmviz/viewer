using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dsmviz.Viewer.View.ValueConverters
{
    public class SearchIndicatorModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSearchActive = (bool)value;
            return isSearchActive ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
