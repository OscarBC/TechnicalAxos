using Microsoft.Maui.Controls;
using System.Globalization;

namespace TechnicalAxos_OscarBarrera.Helpers.Views;

public class FirstCapitalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Verificar si la propiedad es una colección y obtener el primer elemento
        if (value is System.Collections.IEnumerable capitals)
        {
            var firstCapital = capitals.Cast<object>().FirstOrDefault();
            return firstCapital?.ToString() ?? "No Capital"; // Valor por defecto
        }

        return "No Capital"; // Valor por defecto si el valor no es válido
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}