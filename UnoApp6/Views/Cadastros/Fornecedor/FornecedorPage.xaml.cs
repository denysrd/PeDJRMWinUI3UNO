using System.Collections.ObjectModel;
using PeDJRMWinUI3UNO.Services;


namespace PeDJRMWinUI3UNO.Views.Cadastros.Fornecedor;

public sealed partial class FornecedorPage : Page
{
    // Serviço para gerenciar operações de fornecedor
    private readonly FornecedorService _fornecedorService;

    // Coleção observável para armazenar fornecedores e permitir atualização automática na interface
    public ObservableCollection<Models.FornecedorModel> Fornecedores { get; } = new ObservableCollection<Models.FornecedorModel>();

    public FornecedorPage()
    {
        this.InitializeComponent();
        _fornecedorService = (FornecedorService)App.Services.GetService(typeof(FornecedorService));

        // Carrega a lista de fornecedores ao iniciar a página
        _ = CarregarFornecedoresAsync();
    }

    // Método para navegar para a página de cadastro de fornecedor
    private void CadastrarButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CadastroFornecedorPage));
    }

    // Método assíncrono para carregar fornecedores do serviço e atualizar a coleção observável
    public async Task CarregarFornecedoresAsync()
    {
        // Obtém a lista de fornecedores do serviço
        var Listafornecedores = await _fornecedorService.ObterTodosFornecedoresAsync();

        // Limpa e preenche a coleção observável com os fornecedores recebidos
        Fornecedores.Clear();
        foreach (var fornecedor in Listafornecedores)
        {
            Fornecedores.Add(fornecedor);
        }
    }

    // Edita o fornecedor selecionado
    private void EditarButton_Click(object sender, RoutedEventArgs e)
    {
        var fornecedor = (sender as Button)?.Tag as Models.FornecedorModel;
        if (fornecedor != null)
        {
            // Navega para a página de cadastro com os dados do fornecedor
            Frame.Navigate(typeof(CadastroFornecedorPage), fornecedor);
        }
    }

    // Exclui o fornecedor selecionado com confirmação e em escopo transacional
    private async void ExcluirButton_Click(object sender, RoutedEventArgs e)
    {
        var fornecedor = (sender as Button)?.Tag as Models.FornecedorModel;
        if (fornecedor != null)
        {
            // Cria diálogo de confirmação
            var dialog = new ContentDialog
            {
                Title = "Confirmar Exclusão",
                Content = $"Tem certeza de que deseja excluir o fornecedor {fornecedor.Nome}?",
                PrimaryButtonText = "Excluir",
                CloseButtonText = "Cancelar",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    // Chama o serviço para excluir o fornecedor dentro de uma transação
                    await _fornecedorService.ExcluirFornecedorAsync(fornecedor.Id_Fornecedor);

                    // Remove da lista se a exclusão for bem-sucedida
                    Fornecedores.Remove(fornecedor);
                }
                catch (Exception ex)
                {
                    // Exibe uma mensagem de erro caso a exclusão falhe
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro ao Excluir",
                        Content = $"Ocorreu um erro ao tentar excluir o fornecedor: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }
    }
}
