using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace PeDJRMWinUI3UNO.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("F2", CultureInfo.InvariantCulture); // Formata com 2 casas decimais
            }
            return "0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
            {
                return decimalValue;
            }
            return 0m; // Retorna 0 caso a conversão falhe
        }
    }
}
