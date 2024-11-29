using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Receita;

/// Define a lógica da página ReceitasView
public sealed partial class ReceitasView : Page
{
    // Coleção para armazenar receitas carregadas
    public ObservableCollection<ReceitasModel> Receitas { get; set; } = new ObservableCollection<ReceitasModel>();

    // Referência ao serviço de receitas
    private readonly ReceitasService _receitasService;

    public ReceitasView()
    {
        // Inicializa os componentes da página
        this.InitializeComponent();
        // Define o DataContext para esta classe
        this.DataContext = this;

        // Obtém o serviço de receitas do contêiner de injeção de dependência
        _receitasService = App.Services.GetService<ReceitasService>()
            ?? throw new InvalidOperationException("O serviço ReceitasService não foi encontrado.");

        // Carrega as receitas ao inicializar
        _ = CarregarReceitasAsync();
    }

    /// Carrega todas as receitas e suas versões
    /// Carrega todas as receitas e suas versões do banco de dados
    private async Task CarregarReceitasAsync()
    {
        try
        {
            // Obter todas as receitas do banco de dados
            var receitasDoBanco = await _receitasService.ObterTodasReceitasAsync();

            // Limpa a coleção existente antes de adicionar os itens
            Receitas.Clear();

            // Itera pelas receitas para carregar as versões relacionadas
            foreach (var receita in receitasDoBanco)
            {
                // Obter as versões relacionadas à receita
                var versoes = await _receitasService.ObterVersoesReceitaAsync(receita.Id);

                // Adiciona as versões à propriedade da receita
                receita.VersoesReceitas = new ObservableCollection<VersoesReceitasModel>(versoes);

                // Adiciona a receita à coleção observável
                Receitas.Add(receita);

                // Log de depuração para verificar carregamento
                Debug.WriteLine($"Receita carregada: {receita.Nome_Receita} ({receita.Codigo_Receita})  versões.");
            }

            // Log para verificar o número total de receitas carregadas
            Debug.WriteLine($"Total de receitas carregadas: {Receitas.Count}");
        }
        catch (Exception ex)
        {
            // Log do erro e exibição de mensagem ao usuário
            Debug.WriteLine($"Erro ao carregar receitas: {ex.Message}");
            await MostrarDialogoAviso($"Erro ao carregar receitas: {ex.Message}");
        }
    }



    private async Task MostrarDialogoAviso(string mensagem)
    {
        var dialog = new ContentDialog
        {
            Title = "Aviso",
            Content = mensagem,
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }

    /// Método chamado ao digitar no campo de busca.
    /// Atualiza dinamicamente os itens exibidos no ItemsRepeater com base no texto digitado pelo usuário.
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox searchBox) // Verifica se o evento foi disparado por um TextBox.
        {
            var query = searchBox.Text.ToLower(); // Obtém o texto digitado pelo usuário e converte para minúsculas.

            // Caso o campo de busca esteja vazio, restaura a lista completa de receitas.
            if (string.IsNullOrWhiteSpace(query))
            {
                // Redefine o ItemsSource do ItemsRepeater para exibir todas as receitas.
                ReceitasRepeater.ItemsSource = Receitas;
                return; // Sai do método, pois não há necessidade de filtragem.
            }

            // Filtra a coleção de receitas com base no Nome ou Código da Receita.
            var receitasFiltradas = Receitas
                .Where(r => (!string.IsNullOrEmpty(r.Codigo_Receita) && r.Codigo_Receita.ToLower().Contains(query)) || // Filtra por código.
                            (!string.IsNullOrEmpty(r.Nome_Receita) && r.Nome_Receita.ToLower().Contains(query)))       // Filtra por nome.
                .ToList(); // Converte o resultado para uma lista.

            // Atualiza o ItemsRepeater com a lista filtrada.
            ReceitasRepeater.ItemsSource = receitasFiltradas; // Força o ItemsRepeater a exibir apenas os itens filtrados.
        }
    }



    /// Navega para a página de criação de receita
    private void NavigateToNovaReceita(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        Frame.Navigate(typeof(ReceitaPage));
    }

    /// Exclui a receita ou versão selecionada
    private async void ExcluirReceita_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is VersoesReceitasModel versaoParaExcluir)
        {
            // Confirmar exclusão
            var dialog = new ContentDialog
            {
                Title = "Excluir Versão",
                Content = $"Tem certeza de que deseja excluir a versão {versaoParaExcluir.Versao}?",
                PrimaryButtonText = "Excluir",
                CloseButtonText = "Cancelar",
                XamlRoot = this.XamlRoot
            };

            var resultado = await dialog.ShowAsync();
            if (resultado == ContentDialogResult.Primary)
            {
                // Remove a versão da receita
                await _receitasService.RemoverVersaodeReceitaAsync(versaoParaExcluir.Id);
                _ = CarregarReceitasAsync();
            }
        }
    }

    /// Edita uma receita ou versão selecionada
    private void OnEditButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is VersoesReceitasModel versaoParaEditar)
        {
            Frame.Navigate(typeof(ReceitaPage), versaoParaEditar);
        }
    }
}
