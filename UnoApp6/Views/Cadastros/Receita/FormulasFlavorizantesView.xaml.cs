using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Services;
using System.Diagnostics;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Receita;

public sealed partial class FormulaFlavorizanteView : Page
{
    public ObservableCollection<FormulaFlavorizanteModel> Formulas { get; set; } = new ObservableCollection<FormulaFlavorizanteModel>();
    public ObservableCollection<VersoesFormulaFlavorizanteModel> VersoesFormulaFlavorizante { get; set; } = new ObservableCollection<VersoesFormulaFlavorizanteModel>();
    public ObservableCollection<FormulaFlavorizanteInsumosModel> InsumosModels { get; set; } = new ObservableCollection<FormulaFlavorizanteInsumosModel> ();
    // Coleções para os itens da receita e itens disponíveis.
    public ObservableCollection<ItemModel> ItensFormula { get; set; } = new ObservableCollection<ItemModel>();


    private readonly FormulaFlavorizanteService _formulaService;
    private readonly FormulaFlavorizanteInsumosService _formulaFlavorizanteInsumosService;
    

    public FormulaFlavorizanteView()
    {
        InitializeComponent();
        DataContext = this;

        _formulaService = App.Services.GetService<FormulaFlavorizanteService>() ??
            throw new InvalidOperationException("Serviço FormulaFlavorizanteService não encontrado.");
        _formulaFlavorizanteInsumosService = App.Services.GetService<FormulaFlavorizanteInsumosService>() ??
            throw new InvalidOperationException("Serviço FormulaFlavorizanteInsumosService não encontrado.");

        


        _ = CarregarFormulasAsync();
    }


    private async Task CarregarFormulasAsync()
    {
        try
        {
            // Obter todas as receitas do banco de dados
            var formulasDoBanco = await _formulaService.ObterTodasFormulasAsync();

            // Limpa a coleção existente antes de adicionar os itens
            Formulas.Clear();

            // Itera pelas receitas para carregar as versões relacionadas
            foreach (var formula in formulasDoBanco)
            {
                // Obter as versões relacionadas à receita
                var versoes = await _formulaService.ObterVersoesAsync(formula.Id);

                // Para cada versão, carregar os itens associados
                foreach (var versao in versoes)
                {
                    try
                    {
                        // Busca os insumos da versão pelo serviço
                        var formulaInsumos = await _formulaFlavorizanteInsumosService.ObterPorVersaoFormulaIdAsync(versao.Id);

                        // Mapeia os insumos para o modelo ItemModel
                        var itens = formulaInsumos.Select(insumo => new ItemModel
                        {
                            CodigoInterno = insumo.Insumo?.Codigo_Interno ?? insumo.ComponenteAromatico?.CodigoInterno ?? "N/A",
                            Nome = insumo.Insumo?.Nome ?? insumo.ComponenteAromatico?.Nome ?? "Sem nome",
                            Quantidade = insumo.Quantidade,
                            Idinsumo = insumo.Id_Insumo, // Atribui 0 se for nulo
                            idcar = insumo.Id, // Atribui 0 se for nulo
                            UnidadeMedida = insumo.Unidade_Medida // Campo adicionado
                        }).ToList();

                        // Atualiza a propriedade de itens da versão
                        versao.Itens = new ObservableCollection<ItemModel>(itens);
                        Debug.WriteLine($"Itens carregados para a versão {versao.Versao}: {itens.Count}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Erro ao carregar itens para a versão {versao.Versao}: {ex.Message}");
                    }
                }

                // Adiciona as versões à propriedade da receita
                formula.VersoesFormulas = new ObservableCollection<VersoesFormulaFlavorizanteModel>(versoes);

                // Adiciona a receita à coleção observável
                Formulas.Add(formula);

                // Log de depuração para verificar carregamento
                Debug.WriteLine($"Formula carregada: {formula.Nome_Flavorizante} ({formula.Codigo_Flavorizante})  versões.");
            }

            // Log para verificar o número total de receitas carregadas
            Debug.WriteLine($"Total de formula carregadas: {Formulas.Count}");
        }
        catch (Exception ex)
        {
            // Log do erro e exibição de mensagem ao usuário
            Debug.WriteLine($"Erro ao carregar formula: {ex.Message}");
            await MostrarDialogoAviso($"Erro ao carregar formula: {ex.Message}");
        }
    }

    private async void FormulaExpanding(object sender, object e)
    {
        if (sender is Expander expander && expander.DataContext is FormulaFlavorizanteModel formula)
        {
            if (formula.VersoesFormulas.Any()) return;

            try
            {
                var versoes = await _formulaService.ObterVersoesAsync(formula.Id);
                formula.VersoesFormulas = new ObservableCollection<VersoesFormulaFlavorizanteModel>(versoes);
            }
            catch (Exception ex)
            {
                await MostrarDialogoAviso($"Erro ao carregar versões: {ex.Message}");
            }
        }
    }
    
    private async void VersaoExpanding(object sender, object e)
    {
        // Adiciona um atraso de 500ms antes de executar o restante da lógica
        await Task.Delay(500);
        try
        {
            // Verifica se o sender é um Expander e se o DataContext está correto
            if (sender is Expander expander && expander.Tag is VersoesFormulaFlavorizanteModel versao)
            {
                // Verifica se os itens já foram carregados para evitar recarregar
                if (versao.Itens != null && versao.Itens.Any())
                {
                    Debug.WriteLine($"Itens já carregados para a versão {versao.Versao}. Ignorando recarregamento.");
                    return;
                }
                Debug.WriteLine($"Itens carregados para a versão {versao.Versao}: {versao.Itens?.Count ?? 0}");

                // Busca os insumos da versão pelo serviço
                var formuaInsumos = await _formulaFlavorizanteInsumosService.ObterPorVersaoFormulaIdAsync(versao.Id);

                // Mapeia os insumos para o modelo ItemModel
                var itens = formuaInsumos.Select(insumo => new ItemModel
                {
                    CodigoInterno = insumo.Insumo?.Codigo_Interno ?? insumo.Insumo?.Codigo_Interno ?? "N/A",
                    Nome = insumo.Insumo?.Nome ?? insumo.Insumo?.Nome ?? "Sem nome",
                    Quantidade = insumo.Quantidade,
                    Idinsumo = insumo.Id_Insumo,
                    Idflavorizante = insumo.Id_Car
                }).ToList();

                // Atualiza a propriedade de itens da versão
                versao.Itens = new ObservableCollection<ItemModel>(itens);
                Debug.WriteLine($"Itens carregados para a versão {versao.Versao}: {itens.Count}");
            }
            else
            {
                Debug.WriteLine("Expander ou DataContext inválido.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao carregar itens para a versão: {ex.Message}");
            await MostrarDialogoAviso($"Erro ao carregar itens: {ex.Message}");
        }
    }

    // Método chamado ao alterar o texto de busca
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        // Verifica se o TextBox disparador é válido
        if (sender is TextBox textBox)
        {
            // Obtém o texto do campo de busca
            string searchText = textBox.Text;

            // Verifica se o texto não é nulo ou vazio
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                // Chama o serviço para obter os resultados
                var resultados = await _formulaService.ObterPorNomeAsync(searchText);

                // Atualiza a coleção com os resultados
                Formulas.Clear();
                foreach (var formula in resultados)
                {
                    Formulas.Add(formula);
                }
            }
            else
            {
                // Limpa os resultados se a busca estiver vazia
                Formulas.Clear();
            }
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

    private void NavigateToNovaFormula(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(FormulaFlavorizantePage));
    }
}
