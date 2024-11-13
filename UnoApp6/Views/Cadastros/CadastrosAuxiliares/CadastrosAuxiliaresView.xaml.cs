using System.Collections.ObjectModel;
using PeDJRMWinUI3UNO.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PeDJRMWinUI3UNO.Views.Cadastros.CadastrosAuxiliares;

public sealed partial class CadastrosAuxiliaresView : Page
{
    public ObservableCollection<TipoIngredienteModel> TipoIngredientes { get; set; } = new ObservableCollection<TipoIngredienteModel>();
    private readonly TipoIngredienteService _tipoIngredienteService;
    private TipoIngredienteModel _ingredienteEmEdicao;

    public ObservableCollection<TipoFormulacaoModel> TipoFormulacoes { get; set; } = new ObservableCollection<TipoFormulacaoModel>();
    private readonly TipoFormulacaoService _tipoFormulacaoService;
    private TipoFormulacaoModel _formulacaoEmEdicao;

    public CadastrosAuxiliaresView()
    {
        this.InitializeComponent();
        _tipoIngredienteService = App.Services.GetService<TipoIngredienteService>();
        CarregarTipoIngredientesAsync();

        _tipoFormulacaoService = App.Services.GetService<TipoFormulacaoService>();
        CarregarTipoFormulacoesAsync();
    }

    private async Task CarregarTipoFormulacoesAsync()
    {
        var formulacoes = await _tipoFormulacaoService.ObterTodosAsync();
        TipoFormulacoes.Clear();
        foreach (var formulacao in formulacoes)
        {
            TipoFormulacoes.Add(formulacao);
        }
    }

    // Método para verificar se o nome da formulação já existe
    private async Task<bool> VerificarFormulacaoExistenteAsync(string tipoFormula)
    {
        var formulacoes = await _tipoFormulacaoService.ObterTodosAsync();
        var formulacaoExistente = formulacoes.FirstOrDefault(f => f.Tipo_Formula.Equals(tipoFormula, StringComparison.OrdinalIgnoreCase));

        if (formulacaoExistente != null)
        {
            // Exibe um diálogo informando a duplicidade
            var dialog = new ContentDialog
            {
                Title = "Formulação já cadastrada",
                Content = $"Já existe uma formulação com o nome '{tipoFormula}'.",
                CloseButtonText = "Cancelar Operação",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
            LimparCamposCadastroFormulacao();
            return true;
        }
        return false;
    }

    // Evento para iniciar a edição de uma formulação existente
    private void EditarRegistroFormulacao_Click(object sender, RoutedEventArgs e)
    {
        var formulacao = (sender as Button).Tag as TipoFormulacaoModel;

        // Preenche os campos com os dados da formulação selecionada para edição
        TipoFormulaTextBox.Text = formulacao.Tipo_Formula;
        DescricaoFormulaTextBox.Text = formulacao.Descricao_Formula;
        _formulacaoEmEdicao = formulacao;
    }

    // Método para salvar ou atualizar o registro de formulação
    private async void SalvarTipoFormulacao(object sender, RoutedEventArgs e)
    {
        if (_formulacaoEmEdicao != null)
        {
            // Atualiza o registro existente
            _formulacaoEmEdicao.Tipo_Formula = TipoFormulaTextBox.Text;
            _formulacaoEmEdicao.Descricao_Formula = DescricaoFormulaTextBox.Text;

            await _tipoFormulacaoService.AtualizarAsync(_formulacaoEmEdicao);
            _formulacaoEmEdicao = null; // Limpa o item em edição
        }
        else
        {
            var tipoFormula = TipoFormulaTextBox.Text;

            // Verifica se a formulação já existe antes de salvar
            var formulaJaExiste = await VerificarFormulacaoExistenteAsync(tipoFormula);
            if (formulaJaExiste) return;

            // Adiciona um novo registro
            var novaFormulacao = new TipoFormulacaoModel
            {
                Tipo_Formula = tipoFormula,
                Descricao_Formula = DescricaoFormulaTextBox.Text
            };

            await _tipoFormulacaoService.SalvarAsync(novaFormulacao);
            TipoFormulacoes.Add(novaFormulacao);
        }

        await CarregarTipoFormulacoesAsync();
        LimparCamposCadastroFormulacao();
    }

    // Evento para excluir um registro
    private async void ExcluirRegistroFormulacao_Click(object sender, RoutedEventArgs e)
    {
        var formulacao = (sender as Button).Tag as TipoFormulacaoModel;

        var dialog = new ContentDialog
        {
            Title = "Excluir registro",
            Content = $"Tem certeza de que deseja excluir a formulação '{formulacao.Tipo_Formula}'?",
            PrimaryButtonText = "Excluir",
            CloseButtonText = "Cancelar",
            XamlRoot = this.XamlRoot
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            await _tipoFormulacaoService.RemoverAsync(formulacao.Id_Tipo_Formulacao);
            TipoFormulacoes.Remove(formulacao);
        }

        await CarregarTipoFormulacoesAsync();
    }

    private void LimparCamposCadastroFormulacao()
    {
        TipoFormulaTextBox.Text = string.Empty;
        DescricaoFormulaTextBox.Text = string.Empty;
        _formulacaoEmEdicao = null; // Limpa o item em edição
    }

    private async Task CarregarTipoIngredientesAsync()
    {
        var ingredientes = await _tipoIngredienteService.ObterTodosAsync();
        TipoIngredientes.Clear();
        foreach (var ingrediente in ingredientes)
        {
            TipoIngredientes.Add(ingrediente);
        }
    }

    // Método para verificar se a sigla já existe no banco de dados
    private async Task<bool> VerificarSiglaExistenteAsync(string sigla)
    {
        var ingredientes = await _tipoIngredienteService.ObterTodosAsync();
        var ingredienteExistente = ingredientes.FirstOrDefault(i => i.Sigla == sigla.ToUpper());

        if (ingredienteExistente != null)
        {
            // Mostra diálogo com informações do registro duplicado
            var dialog = new ContentDialog
            {
                Title = "Sigla já cadastrada",
                Content = $"Já existe um registro com a sigla '{sigla}'.\n\n" +
                          $"Tipo: {ingredienteExistente.Tipo_Ingrediente}\n" +
                          $"Descrição: {ingredienteExistente.Descricao_Tipo_Ingrediente}\n" +
                          $"Situação: {(ingredienteExistente.Situacao ? "Ativo" : "Inativo")}\n" +
                          $"Sigla: {ingredienteExistente.Sigla}",
                CloseButtonText = "Cancelar Operação",
                CloseButtonStyle = Application.Current.Resources["PrimaryButtonStyle"] as Style,
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
            LimparCamposCadastro(); // Limpa os campos após o aviso de duplicidade
            return true;
        }
        return false;
    }

    // Evento para iniciar a edição de um registro
    private void EditarRegistroIngrediente_Click(object sender, RoutedEventArgs e)
    {
        var ingrediente = (sender as Button).Tag as TipoIngredienteModel;

        // Preenche os campos com os dados do item selecionado para edição
        TipoIngredienteTextBox.Text = ingrediente.Tipo_Ingrediente;
        DescricaoTipoIngredienteTextBox.Text = ingrediente.Descricao_Tipo_Ingrediente;
        SiglaTextBox.Text = ingrediente.Sigla.ToUpper(); // Converte a sigla para caixa alta
        SituacaoToggleSwitch.IsOn = ingrediente.Situacao;

        _ingredienteEmEdicao = ingrediente; // Armazena o item em edição
    }

    // Monitora as alterações no campo Sigla para limitar o número de caracteres e exibir sugestões
    private async void SiglaTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox.Text.Length > 3)
        {
            // Trunca o valor para 3 caracteres e converte para caixa alta
            textBox.Text = textBox.Text.Substring(0, 3).ToUpper();
            textBox.SelectionStart = textBox.Text.Length;

            var tipoIngrediente = TipoIngredienteTextBox.Text;
            if (!string.IsNullOrEmpty(tipoIngrediente))
            {
                var sugestoes = GerarSugestoesSigla(tipoIngrediente);
                var listView = new ListView { ItemsSource = sugestoes };
                listView.ItemClick += (s, args) => { textBox.Text = args.ClickedItem.ToString().ToUpper(); };

                var dialog = new ContentDialog
                {
                    Title = "Máximo de 3 caracteres para a Sigla",
                    Content = listView,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
        else
        {
            // Converte o texto atual para caixa alta
            textBox.Text = textBox.Text.ToUpper();
            textBox.SelectionStart = textBox.Text.Length;
        }
    }

    // Gera uma lista de sugestões para a sigla com base no tipo de ingrediente
    private List<string> GerarSugestoesSigla(string tipoIngrediente)
    {
        return tipoIngrediente.Split(' ')
            .Select(p => p.Substring(0, Math.Min(3, p.Length)).ToUpper())
            .Distinct()
            .ToList();
    }

    // Método para salvar ou atualizar o registro
    private async void SalvarTipoIngrediente(object sender, RoutedEventArgs e)
    {
        bool situacaoAtual = SituacaoToggleSwitch.IsOn;
        var siglaUpperCase = SiglaTextBox.Text.ToUpper(); // Converte a sigla para caixa alta

        if (_ingredienteEmEdicao != null)
        {
            // Atualiza o registro existente
            _ingredienteEmEdicao.Tipo_Ingrediente = TipoIngredienteTextBox.Text;
            _ingredienteEmEdicao.Descricao_Tipo_Ingrediente = DescricaoTipoIngredienteTextBox.Text;
            _ingredienteEmEdicao.Situacao = situacaoAtual;
            _ingredienteEmEdicao.Sigla = siglaUpperCase;

            await _tipoIngredienteService.AtualizarAsync(_ingredienteEmEdicao);
            _ingredienteEmEdicao = null; // Limpa o item em edição
        }
        else
        {
            var siglaJaExiste = await VerificarSiglaExistenteAsync(siglaUpperCase);
            if (siglaJaExiste) return; // Se a sigla já existe, cancela o salvamento

            var novoIngrediente = new TipoIngredienteModel
            {
                Tipo_Ingrediente = TipoIngredienteTextBox.Text,
                Descricao_Tipo_Ingrediente = DescricaoTipoIngredienteTextBox.Text,
                Situacao = situacaoAtual,
                Sigla = siglaUpperCase // Salva a sigla em caixa alta
            };

            await _tipoIngredienteService.SalvarAsync(novoIngrediente);
            TipoIngredientes.Add(novoIngrediente);
        }

        await CarregarTipoIngredientesAsync();
        LimparCamposCadastro();
    }

    // Evento para excluir um registro
    private async void ExcluirRegistroIngrediente_Click(object sender, RoutedEventArgs e)
    {
        var ingrediente = (sender as Button).Tag as TipoIngredienteModel;

        var dialog = new ContentDialog
        {
            Title = "Excluir registro",
            Content = $"Tem certeza de que deseja excluir o registro '{ingrediente.Tipo_Ingrediente}'?",
            PrimaryButtonText = "Excluir",
            CloseButtonText = "Cancelar",
            XamlRoot = this.XamlRoot
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            await _tipoIngredienteService.RemoverAsync(ingrediente);
            TipoIngredientes.Remove(ingrediente);
        }

        await CarregarTipoIngredientesAsync();
    }

    private void LimparCamposCadastro()
    {
        TipoIngredienteTextBox.Text = string.Empty;
        DescricaoTipoIngredienteTextBox.Text = string.Empty;
        SiglaTextBox.Text = string.Empty;
        SituacaoToggleSwitch.IsOn = false;
        _ingredienteEmEdicao = null; // Limpa o item em edição
    }
}
