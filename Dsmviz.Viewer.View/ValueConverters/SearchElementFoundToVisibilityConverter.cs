using Dsmviz.Interfaces.Data.Entities;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dsmviz.Viewer.View.ValueConverters
{
    public class SearchElementFoundToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IElement foundElement = (IElement)value;
            return (foundElement != null) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
