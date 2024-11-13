
using System.Text.RegularExpressions;
using PeDJRMWinUI3UNO.Services;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Fornecedor;

public sealed partial class CadastroFornecedorPage : Page
{
    private readonly FornecedorService _fornecedorService;

    public CadastroFornecedorPage()
    {
        this.InitializeComponent();
        _fornecedorService = (FornecedorService)App.Services.GetService(typeof(FornecedorService));
    }

    private void DocumentoTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Lógica para formatar o documento enquanto o usuário digita
        string documento = Regex.Replace(DocumentoTextBox.Text, @"[^\d]", "");
        if (documento.Length > 14) documento = documento.Substring(0, 14);
        int selectionStart = DocumentoTextBox.Text.Length;
        if (documento.Length <= 11) // CPF
        {
            if (documento.Length > 3) documento = documento.Insert(3, ".");
            if (documento.Length > 7) documento = documento.Insert(7, ".");
            if (documento.Length > 11) documento = documento.Insert(11, "-");
        }
        else // CNPJ
        {
            if (documento.Length > 2) documento = documento.Insert(2, ".");
            if (documento.Length > 6) documento = documento.Insert(6, ".");
            if (documento.Length > 10) documento = documento.Insert(10, "/");
            if (documento.Length > 15) documento = documento.Insert(15, "-");
        }
        DocumentoTextBox.Text = documento;
        DocumentoTextBox.SelectionStart = selectionStart;
    }

    private async void SalvarButton_Click(object sender, RoutedEventArgs e)
    {
        // Validação e obtenção de dados
        if (string.IsNullOrWhiteSpace(NomeTextBox.Text) || string.IsNullOrWhiteSpace(DocumentoTextBox.Text))
        {
            await ShowMessageAsync("Nome e Documento são obrigatórios.");
            return;
        }

        var fornecedor = new Models.FornecedorModel
        {
            Nome = NomeTextBox.Text,
            Documento = DocumentoTextBox.Text,
            Email = EmailTextBox.Text,
            Telefone = TelefoneTextBox.Text,
           // Situacao = SituacaoToggleSwitch.IsOn
        };

        bool sucesso = await _fornecedorService.InserirFornecedorAsync(fornecedor);
        if (sucesso)
        {
            await ShowMessageAsync("Fornecedor cadastrado com sucesso!");
            LimparCampos();
            // Navega de volta para a FornecedorPage após salvar com sucesso
            Frame.Navigate(typeof(FornecedorPage));
        }
        else
        {
            await ShowMessageAsync("Erro ao cadastrar o fornecedor.");
        }
    }

    private async Task ShowMessageAsync(string message)
    {
        var dialog = new ContentDialog
        {
            Title = "Informação",
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }

    private void LimparCampos()
    {
        NomeTextBox.Text = string.Empty;
        DocumentoTextBox.Text = string.Empty;
        EmailTextBox.Text = string.Empty;
        TelefoneTextBox.Text = string.Empty;
        SituacaoToggleSwitch.IsOn = false;
    }

    private void CancelarButton_Click(object sender, RoutedEventArgs e)
    {
        // Navega para a página anterior
        if (Frame.CanGoBack)
        {
            Frame.GoBack();
        }
    }
}
