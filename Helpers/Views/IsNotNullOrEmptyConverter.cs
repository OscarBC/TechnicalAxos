using Microsoft.Maui.Controls;
using System.Globalization;

namespace TechnicalAxos_OscarBarrera.Helpers.Views;

public class IsNotNullOrEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string strValue)
        {
            return !string.IsNullOrEmpty(strValue);
        }
        return value != null; // Returns true if the value is not null
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // ConvertBack is not needed for one-way bindings
    }
}
