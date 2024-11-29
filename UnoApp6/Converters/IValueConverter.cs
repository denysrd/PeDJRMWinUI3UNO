using Microsoft.UI.Xaml.Data;
using System;

namespace PeDJRMWinUI3UNO.Converters;

public class DateFormatConverter : IValueConverter
{
    /// <summary>
    /// Converte o valor para o formato de data desejado.
    /// </summary>
    /// <param name="value">O valor a ser formatado.</param>
    /// <param name="targetType">O tipo do destino.</param>
    /// <param name="parameter">Parâmetro opcional.</param>
    /// <param name="language">Idioma atual.</param>
    /// <returns>A data formatada como string.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime date)
        {
            return date.ToString("dd/MM/yyyy"); // Formata a data como "dd/MM/yyyy".
        }
        return string.Empty; // Retorna vazio se o valor não for uma data válida.
    }

    /// <summary>
    /// Método necessário, mas não utilizado.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
