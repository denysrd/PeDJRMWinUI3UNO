using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Receita;

public sealed partial class ReceitasView : Page
{
    public ObservableCollection<ReceitaModel> Receitas { get; set; } = new ObservableCollection<ReceitaModel>();

    public ReceitasView()
    {
        this.InitializeComponent();
        this.DataContext = this;

        // Carregar receitas cadastradas
        CarregarReceitas();
    }

    private void CarregarReceitas()
    {
        // Aqui você pode carregar receitas do banco de dados ou de outra fonte de dados
        Receitas.Add(new ReceitaModel { Nome = "Receita 1", DataCriacao = "2024-11-18" });
        Receitas.Add(new ReceitaModel { Nome = "Receita 2", DataCriacao = "2024-11-17" });
        Receitas.Add(new ReceitaModel { Nome = "Receita 3", DataCriacao = "2024-11-16" });

        // Vincular a lista ao ListView
        ReceitasListView.ItemsSource = Receitas;
    }

    private void NavigateToNovaReceita(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        // Navegar para ReceitaPage.xaml para criar uma nova receita
        Frame.Navigate(typeof(ReceitaPage));
    }

    private void ReceitasListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Exemplo: Ação ao selecionar uma receita (como editar)
        if (ReceitasListView.SelectedItem is ReceitasModel receita)
        {
            // Implementar lógica de edição aqui, se necessário
        }
    }
}

public class ReceitaModel
{
    public string Nome { get; set; }
    public string DataCriacao { get; set; }
}
