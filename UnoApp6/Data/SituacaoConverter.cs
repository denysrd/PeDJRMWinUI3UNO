using Microsoft.UI.Xaml.Data;
// Namespace: PeDJRMWinUI3UNO.Converters
namespace PeDJRMWinUI3UNO.Data;

public class SituacaoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (bool)value ? "Ativo" : "Inativo";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
