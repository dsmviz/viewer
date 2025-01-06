using Dsmviz.Interfaces.Util;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dsmviz.Viewer.View.ValueConverters
{
    public class ProgressStateBusyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ProgressState progressState = (ProgressState)value;
            return (progressState == ProgressState.Busy) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
