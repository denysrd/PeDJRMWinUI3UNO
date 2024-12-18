using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using PeDJRMWinUI3UNO.Services;


namespace PeDJRMWinUI3UNO.Views.Cadastros;

public sealed partial class InsumosView : Page
{
    private readonly InsumosService _insumosService;
    private readonly TipoIngredienteService _tipoIngredienteService;
    private readonly FornecedorService _fornecedorService;

    private InsumoModel insumoEmEdicao;

    public ObservableCollection<TipoIngredienteModel> TipoIngredientes { get; set; } = new ObservableCollection<TipoIngredienteModel>();
    public ObservableCollection<FornecedorModel> Fornecedores { get; set; } = new ObservableCollection<FornecedorModel>();
    public ObservableCollection<InsumoModel> Insumos { get; set; } = new ObservableCollection<InsumoModel>();

    public InsumosView()
    {
        this.InitializeComponent();

        _insumosService = App.Services.GetService<InsumosService>();
        _tipoIngredienteService = App.Services.GetService<TipoIngredienteService>();
        _fornecedorService = App.Services.GetService<FornecedorService>();

        if (_insumosService == null || _tipoIngredienteService == null || _fornecedorService == null)
        {
            MostrarDialogoAviso("Erro ao carregar os serviços. Verifique a configuração do serviço.");
            return;
        }

        CarregarDadosAsync();
    }

    private async void CarregarDadosAsync()
    {
        TipoIngredientes.Clear();
        Fornecedores.Clear();

        var tipoIngredientes = await _tipoIngredienteService.ObterTodosAsync();
        if (tipoIngredientes != null && tipoIngredientes.Any())
        {
            foreach (var tipo in tipoIngredientes)
            {
                TipoIngredientes.Add(tipo);
            }
        }
        else
        {
            await MostrarDialogoAviso("Nenhum tipo de ingrediente cadastrado.");
        }
        SiglaComboBox.ItemsSource = TipoIngredientes;

        var fornecedores = await _fornecedorService.ObterTodosFornecedoresAsync();
        if (fornecedores != null && fornecedores.Any())
        {
            foreach (var fornecedor in fornecedores)
            {
                Fornecedores.Add(fornecedor);
            }
        }
        else
        {
            await MostrarDialogoAviso("Nenhum fornecedor cadastrado.");
        }
        FornecedorComboBox.ItemsSource = Fornecedores;

        // Recria a coleção de insumos
        Insumos = new ObservableCollection<InsumoModel>(await _insumosService.ObterTodosAsync());
        InsumoDataGrid.ItemsSource = Insumos;
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

    private async void AtualizarCodigoInterno(object sender, SelectionChangedEventArgs e)
    {
        // Verifica se estamos no modo de edição
        if (insumoEmEdicao != null)
        {
            // Atualiza apenas a sigla ou o ID do fornecedor no código interno atual
            string[] partesCodigo = insumoEmEdicao.Codigo_Interno.Split('-');
            if (partesCodigo.Length == 3)
            {
                string sigla = partesCodigo[0];
                string id = partesCodigo[1]; // mantém o ID original
                string idFornecedor = partesCodigo[2];

                // Atualiza sigla ou ID do fornecedor conforme seleção atual
                if (SiglaComboBox.SelectedItem is TipoIngredienteModel tipoIngrediente)
                {
                    sigla = tipoIngrediente.Sigla;
                }
                if (FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
                {
                    idFornecedor = fornecedor.Id_Fornecedor.ToString();
                }

                // Define o código atualizado no campo
                CodigoInternoTextBox.Text = $"{sigla}-{id}-{idFornecedor}";
            }
        }
        else
        {
            // Gera um novo código interno ao criar um novo registro
            string sigla = "";
            string idFornecedor = "";

            if (SiglaComboBox.SelectedItem is TipoIngredienteModel tipoIngrediente)
            {
                sigla = tipoIngrediente.Sigla;
            }
            if (FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
            {
                idFornecedor = fornecedor.Id_Fornecedor.ToString();
            }

            // Obtém o próximo ID para criação de um novo código
            int proximoId = await _insumosService.ObterProximoIdAsync();
            CodigoInternoTextBox.Text = $"{sigla}-{proximoId}-{idFornecedor}";
        }
    }

    private async void SalvarInsumo_Click(object sender, RoutedEventArgs e)
    {
        if (SiglaComboBox.SelectedItem is TipoIngredienteModel tipoIngrediente && FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
        {
            bool novoRegistro = insumoEmEdicao == null;

            if (novoRegistro)
            {
                // Criando novo registro
                insumoEmEdicao = new InsumoModel
                {
                    Id_Insumo = await _insumosService.ObterProximoIdAsync(),
                    Codigo_Interno = CodigoInternoTextBox.Text
                };
            }
            else
            {
                // Atualiza o Código Interno antes de salvar, para que o valor correto seja salvo
                insumoEmEdicao.Codigo_Interno = CodigoInternoTextBox.Text;
            }

            // Atualizando os campos do insumo com as novas informações
            insumoEmEdicao.Nome = NomeTextBox.Text;
            insumoEmEdicao.Custo = decimal.TryParse(CustoTextBox.Text, out var custo) ? custo : 0;
            insumoEmEdicao.Descricao_Insumo = DescricaoInsumoTextBox.Text;
            insumoEmEdicao.codigo_produto_fornecedor = CodigoFornecedorTextBox.Text;
            insumoEmEdicao.Id_Fornecedor = fornecedor.Id_Fornecedor;
            insumoEmEdicao.IdTipoIngrediente = tipoIngrediente.Id_Tipo_Ingrediente;
            insumoEmEdicao.Situacao = SituacaoToggleSwitch.IsOn; // Salva o estado do ToggleButton

            var insumoComCodigoFornecedor = Insumos.FirstOrDefault(i => i.codigo_produto_fornecedor == insumoEmEdicao.codigo_produto_fornecedor && i.Id_Insumo != insumoEmEdicao.Id_Insumo);
            if (insumoComCodigoFornecedor != null)
            {
                await MostrarDialogoAviso($"O código produto do fornecedor '{insumoEmEdicao.codigo_produto_fornecedor}' já está cadastrado para o insumo:\n\n" +
                                          $"Nome: {insumoComCodigoFornecedor.Nome}\n" +
                                          $"Descrição: {insumoComCodigoFornecedor.Descricao_Insumo}\n" +
                                          $"Fornecedor ID: {insumoComCodigoFornecedor.Id_Fornecedor}\n" +
                                          $"Custo: {insumoComCodigoFornecedor.Custo}");
                LimparCampos();
                return;
            }

            bool sucesso;
            if (novoRegistro) // Novo insumo
            {
                sucesso = await _insumosService.SalvarAsync(insumoEmEdicao);
                if (sucesso)
                {
                    Insumos.Add(insumoEmEdicao);
                }
            }
            else // Atualização de insumo existente
            {
                sucesso = await _insumosService.AtualizarAsync(insumoEmEdicao);
                if (sucesso)
                {
                    // Remove e re-insere o item atualizado na coleção para forçar a atualização do DataGrid
                    var index = Insumos.IndexOf(insumoEmEdicao);
                    if (index >= 0)
                    {
                        Insumos.RemoveAt(index);
                        Insumos.Insert(index, insumoEmEdicao);
                    }

                    // Recria a coleção e redefine a origem de dados do DataGrid
                    Insumos = new ObservableCollection<InsumoModel>(Insumos);
                    InsumoDataGrid.ItemsSource = Insumos;
                }
            }

            if (sucesso)
            {
                LimparCampos();
                insumoEmEdicao = null; // Reseta o insumoEmEdicao após salvar ou atualizar
            }
            else
            {
                await MostrarDialogoAviso("Falha ao salvar ou atualizar o insumo. Verifique os campos.");
            }
        }
    }




    private void EditarInsumo_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var insumo = button?.Tag as InsumoModel;

        if (insumo != null)
        {
            insumoEmEdicao = insumo;
            NomeTextBox.Text = insumo.Nome;
            CustoTextBox.Text = insumo.Custo.ToString();
            CodigoInternoTextBox.Text = insumo.Codigo_Interno; // Carrega o código interno original
            DescricaoInsumoTextBox.Text = insumo.Descricao_Insumo;
            CodigoFornecedorTextBox.Text = insumo.codigo_produto_fornecedor;

            SiglaComboBox.SelectedItem = TipoIngredientes.FirstOrDefault(t => t.Id_Tipo_Ingrediente == insumo.IdTipoIngrediente);
            FornecedorComboBox.SelectedItem = Fornecedores.FirstOrDefault(f => f.Id_Fornecedor == insumo.Id_Fornecedor);

            // Define o estado do ToggleButton com base na propriedade "Situacao" do insumo
            SituacaoToggleSwitch.IsOn = insumo.Situacao;            
        }
    }

    private void LimparCampos()
    {
        NomeTextBox.Text = string.Empty;
        CustoTextBox.Text = string.Empty;
        SiglaComboBox.SelectedIndex = -1;
        FornecedorComboBox.SelectedIndex = -1;
        CodigoInternoTextBox.Text = string.Empty;
        DescricaoInsumoTextBox.Text = string.Empty;
        CodigoFornecedorTextBox.Text = string.Empty;
        insumoEmEdicao = null;
    }
}
