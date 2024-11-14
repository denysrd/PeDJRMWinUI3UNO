using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using PeDJRMWinUI3UNO.Services;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Fornecedor
{
    public sealed partial class FornecedorPage : Page
    {
        private readonly FornecedorService _fornecedorService;
        public ObservableCollection<Models.FornecedorModel> Fornecedores { get; } = new ObservableCollection<Models.FornecedorModel>();
        private Models.FornecedorModel _fornecedorSelecionado; // Fornecedor selecionado para edição

        public FornecedorPage()
        {
            this.InitializeComponent();
            _fornecedorService = (FornecedorService)App.Services.GetService(typeof(FornecedorService));
            _ = CarregarFornecedoresAsync();
        }

        private void DocumentoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
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
            if (string.IsNullOrWhiteSpace(NomeTextBox.Text) || string.IsNullOrWhiteSpace(DocumentoTextBox.Text))
            {
                await ShowMessageAsync("Nome e Documento são obrigatórios.");
                return;
            }

            if (_fornecedorSelecionado == null)
            {
                // Inserção de novo fornecedor
                var fornecedor = new Models.FornecedorModel
                {
                    Nome = NomeTextBox.Text,
                    Documento = DocumentoTextBox.Text,
                    Email = EmailTextBox.Text,
                    Telefone = TelefoneTextBox.Text,
                    Situacao = SituacaoToggleSwitch.IsOn
                };

                bool sucesso = await _fornecedorService.InserirFornecedorAsync(fornecedor);
                if (sucesso)
                {
                    await ShowMessageAsync("Fornecedor cadastrado com sucesso!");
                    Fornecedores.Add(fornecedor);
                    LimparCampos();
                }
                else
                {
                    await ShowMessageAsync("Erro ao cadastrar o fornecedor.");
                }
            }
            else
            {
                // Atualização de fornecedor existente
                _fornecedorSelecionado.Nome = NomeTextBox.Text;
                _fornecedorSelecionado.Documento = DocumentoTextBox.Text;
                _fornecedorSelecionado.Email = EmailTextBox.Text;
                _fornecedorSelecionado.Telefone = TelefoneTextBox.Text;
                _fornecedorSelecionado.Situacao = SituacaoToggleSwitch.IsOn;

                bool sucesso = await _fornecedorService.AtualizarFornecedorAsync(_fornecedorSelecionado);
                if (sucesso)
                {
                    await ShowMessageAsync("Fornecedor atualizado com sucesso!");
                    LimparCampos();
                }
                else
                {
                    await ShowMessageAsync("Erro ao atualizar o fornecedor.");
                }
            }
            _fornecedorSelecionado = null; // Limpar seleção após salvar
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
            _fornecedorSelecionado = null; // Limpa o fornecedor selecionado após limpar os campos
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public async Task CarregarFornecedoresAsync()
        {
            var listaFornecedores = await _fornecedorService.ObterTodosFornecedoresAsync();
            Fornecedores.Clear();
            foreach (var fornecedor in listaFornecedores)
            {
                Fornecedores.Add(fornecedor);
            }
        }

        private void EditarButton_Click(object sender, RoutedEventArgs e)
        {
            _fornecedorSelecionado = (sender as Button)?.Tag as Models.FornecedorModel;
            if (_fornecedorSelecionado != null)
            {
                NomeTextBox.Text = _fornecedorSelecionado.Nome;
                DocumentoTextBox.Text = _fornecedorSelecionado.Documento;
                EmailTextBox.Text = _fornecedorSelecionado.Email;
                TelefoneTextBox.Text = _fornecedorSelecionado.Telefone;
                SituacaoToggleSwitch.IsOn = _fornecedorSelecionado.Situacao;
            }
        }

        private async void ExcluirButton_Click(object sender, RoutedEventArgs e)
        {
            var fornecedor = (sender as Button)?.Tag as Models.FornecedorModel;
            if (fornecedor != null)
            {
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
                        await _fornecedorService.ExcluirFornecedorAsync(fornecedor.Id_Fornecedor);
                        Fornecedores.Remove(fornecedor);
                    }
                    catch (Exception ex)
                    {
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
}
