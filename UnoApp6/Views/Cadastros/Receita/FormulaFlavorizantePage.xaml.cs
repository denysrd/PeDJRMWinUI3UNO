using System.Collections.ObjectModel;
using PeDJRMWinUI3UNO.Services;
using System.Diagnostics;
using Microsoft.UI.Xaml.Input;
using System.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;




using SkiaSharp;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PeDJRMWinUI3UNO.Views.Cadastros.Receita;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class FormulaFlavorizantePage : Page
{
    // Evento para notificar mudanças nas propriedades.
    // Inicializado com um delegate vazio para evitar nulidade.
    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    /// Método auxiliar para disparar o evento PropertyChanged.
    /// <param name="propertyName">O nome da propriedade que foi alterada.</param>
    private void OnPropertyChanged(string propertyName)
    {
        // Dispara o evento PropertyChanged para notificar a interface.
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Serviços injetados para manipular formulas e insumos.
    private readonly InsumosService _insumoService;
    private readonly FlavorizantesService _flavorizanteService;
    private readonly ComponenteAromaticoService _componenteAromaticoService;
    private readonly FormulaFlavorizanteService _formulaFlavorizanteService;
    private readonly VersoesFormulaFlavorizanteService _versoesFormulaFlavorizanteService;
    private readonly FormulaFlavorizanteInsumosService _formulaFlavorizanteInsumosService;

    /// Coleção de séries para exibição no gráfico de pizza.
    public ObservableCollection<ISeries> PieSeries { get; set; } = new ObservableCollection<ISeries>();

    // Coleções para armazenar dados de insumos, flavorizantes, receitas, etc.
    public ObservableCollection<InsumoModel> Insumos { get; set; } = new ObservableCollection<InsumoModel>();
    public ObservableCollection<FlavorizantesModel> Flavorizantes { get; set; } = new ObservableCollection<FlavorizantesModel>();
    public ObservableCollection<FormulaFlavorizanteModel> Formulas { get; set; } = new ObservableCollection<FormulaFlavorizanteModel>();
    public ObservableCollection<VersoesFormulaFlavorizanteModel> VersoesFormulas { get; set; } = new ObservableCollection<VersoesFormulaFlavorizanteModel>();
    public ObservableCollection<FormulaFlavorizanteInsumosModel> FormulasInsumos { get; set; } = new ObservableCollection<FormulaFlavorizanteInsumosModel>();
    public ObservableCollection<ComponenteAromaticoModel> ComponentesAromaticos{ get; set; } = new ObservableCollection<ComponenteAromaticoModel>();


    // Coleções para os itens da receita e itens disponíveis.
    public ObservableCollection<ItemModel> ItensFormula { get; set; } = new ObservableCollection<ItemModel>();
    public ObservableCollection<ItemModel> ItensDisponiveis { get; set; } = new ObservableCollection<ItemModel>();

    private bool _isNewFormula = true; // Define como novo por padrão

    /// Construtor da classe ReceitaPage.
    /// Configura os serviços injetados e inicializa os dados.
    public FormulaFlavorizantePage()
    {
        this.InitializeComponent();
        this.DataContext = this; // Configura o DataContext para a própria página.

        // Injeção de dependência para os serviços.
        _insumoService = App.Services.GetService(typeof(InsumosService)) as InsumosService
            ?? throw new InvalidOperationException("O serviço InsumosService não foi encontrado no contêiner de injeção de dependência.");
        _flavorizanteService = App.Services.GetService(typeof(FlavorizantesService)) as FlavorizantesService
            ?? throw new InvalidOperationException("O serviço FlavorizantesService não foi encontrado no contêiner de injeção de dependência.");
        _componenteAromaticoService = App.Services.GetService(typeof(ComponenteAromaticoService)) as ComponenteAromaticoService
            ?? throw new InvalidOperationException("O serviço ReceitasService não foi encontrado no contêiner de injeção de dependência.");
        _formulaFlavorizanteService = App.Services.GetService(typeof(FormulaFlavorizanteService)) as FormulaFlavorizanteService
            ?? throw new InvalidOperationException("O serviço VersoesReceitasService não foi encontrado no contêiner de injeção de dependência.");
        _versoesFormulaFlavorizanteService = App.Services.GetService(typeof(VersoesFormulaFlavorizanteService)) as VersoesFormulaFlavorizanteService
            ?? throw new InvalidOperationException("O serviço ReceitasInsumosService não foi encontrado no contêiner de injeção de dependência.");
        _formulaFlavorizanteInsumosService = App.Services.GetService(typeof(FormulaFlavorizanteInsumosService)) as FormulaFlavorizanteInsumosService
            ?? throw new InvalidOperationException("O serviço ReceitasInsumosService não foi encontrado no contêiner de injeção de dependência.");

        // Carrega os itens disponíveis e inicializa a lista.
        _ = CarregarItensDisponiveisAsync();
        AdicionarLinhaVazia();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        try
        {
            if (e.Parameter is FormulaFlavorizanteModel formulaParaCopia)
            {
                // Trata a cópia da receita
                await CriarNovaFormulaBaseadaEmCopiaAsync(formulaParaCopia);
            }
            else if (e.Parameter is VersoesFormulaFlavorizanteModel versaoParaEdicao)
            {
                // Trata a edição de uma versão existente
                await CarregarDadosFormulaAsync(versaoParaEdicao);
            }
            else
            {
                Debug.WriteLine("Parâmetro inválido recebido.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao navegar para a página: {ex.Message}");
        }
    }


    /// <summary>
    /// Carrega os dados da receita e preenche a interface para edição.
    /// </summary>
    /// <param name="versaoSelecionada">A versão da receita selecionada.</param>
    private async Task CarregarDadosFormulaAsync(VersoesFormulaFlavorizanteModel versaoSelecionada)
    {
        // Define que não estamos criando uma nova receita.
        _isNewFormula = false;

        // Obtém os dados da receita associados à versão selecionada.
        var formula = await _formulaFlavorizanteService.ObterPorIdAsync(versaoSelecionada.Id_Formula_Flavorizante);

        if (formula == null)
        {
            Debug.WriteLine("Formula não encontrada.");
            return;
        }

        // Preenche os campos da interface com os dados da receita.
        CodigoFormulaTextBox.Text = formula.Codigo_Flavorizante;
        NomeFormulaTextBox.Text = formula.Nome_Flavorizante;
        DescricaoFormulaTextBox.Text = versaoSelecionada.Descricao_Processo;
        DataFormulaPicker.Date = versaoSelecionada.Data;

        // Obtém os itens da receita.
        var itensFormula = await _formulaFlavorizanteInsumosService.ObterPorVersaoFormulaIdAsync(versaoSelecionada.Id);

        // Preenche os itens na interface.
        PreencherItensFormula(itensFormula);
    }

    /// <summary>
    /// Cria uma nova Formula baseada em uma cópia existente.
    /// </summary>
    /// <param name="receita">A Formula a ser copiada.</param>
    private async Task CriarNovaFormulaBaseadaEmCopiaAsync(FormulaFlavorizanteModel formula)
    {
        // Define que estamos criando uma nova Formula.
        _isNewFormula = true;

        // Gera um novo código para a Formula copiada.
        CodigoFormulaTextBox.Text = await GerarCodigoFormulaFlavorizanteAsync();

        // Preenche os campos com os dados da Formula copiada.
        NomeFormulaTextBox.Text = string.Empty; // Limpa o nome para que o usuário insira um novo.
        DescricaoFormulaTextBox.Text = string.Empty; // Limpa a descrição para uma nova entrada.
        DataFormulaPicker.Date = DateTimeOffset.Now; // Define a data atual.

        // Obtém a última versão da Formula copiada.
        var ultimaVersao = (await _versoesFormulaFlavorizanteService.ObterPorFormulaIdAsync(formula.Id))
            .OrderByDescending(v => v.Versao)
            .FirstOrDefault();

        if (ultimaVersao != null)
        {
            var itensFormula = await _formulaFlavorizanteInsumosService.ObterPorVersaoFormulaIdAsync(ultimaVersao.Id);

            // Preenche os itens na interface.
            PreencherItensFormula(itensFormula);
        }
        else
        {
            Debug.WriteLine("Nenhuma versão encontrada para a receita a ser copiada.");
        }
    }

    /// <summary>
    /// Preenche os itens da Formula na interface.
    /// </summary>
    /// <param name="itensFormula">A lista de itens da Formula.</param>
    private void PreencherItensFormula(IEnumerable<FormulaFlavorizanteInsumosModel> itensFormula)
    {
        // Limpa a lista de itens antes de adicionar os novos dados.
        ItensFormula.Clear();

        // Calcula a soma total das quantidades para o cálculo das porcentagens.
        decimal somaQuantidades = itensFormula.Sum(item => item.Quantidade);

        // Itera sobre os itens retornados e os adiciona à interface.
        foreach (var item in itensFormula)
        {
            decimal porcentagem = somaQuantidades > 0
                ? Math.Round((item.Quantidade / somaQuantidades) * 100, 3)
                : 0;

            ItensFormula.Add(new ItemModel
            {
                Nome = item.Insumo?.Nome ?? item.ComponenteAromatico.Nome ?? string.Empty,
                CodigoInterno = item.Insumo?.Codigo_Interno ?? item.ComponenteAromatico.CodigoInterno ?? string.Empty,
                Quantidade = item.Quantidade,
                UnidadeMedida = item.Unidade_Medida,
                Idinsumo = item.Id_Insumo,
                idcar = item.Id_Car,                
                Porcentagem = porcentagem
            });
        }

        // Define o item selecionado no ComboBox de unidade de medida.
        UnidadeMedidaComboBox.SelectedItem = UnidadeMedidaComboBox.Items
            .OfType<ComboBoxItem>()
            .FirstOrDefault(i => i.Content?.ToString() == ItensFormula.FirstOrDefault()?.UnidadeMedida);

        // Atualiza o gráfico com os dados carregados.
        AtualizarGrafico();
    }



    private async void SalvarFormula_Click(object sender, RoutedEventArgs e)
    {
        await SalvarFormulaAsync();
    }

    /// Método responsável por salvar a Formula.
    /// Ao editar, cria uma nova versão. Ao criar, adiciona uma nova Formula.
    private async Task SalvarFormulaAsync()
    {
        try
        {
            // Verifica se o nome da Formula foi preenchido.
            if (string.IsNullOrWhiteSpace(NomeFormulaTextBox.Text))
            {
                await MostrarDialogoAviso("O nome da formula é obrigatório.");
                return;
            }

            // Preenche o modelo da Formula com os dados da interface.
            var formula = new FormulaFlavorizanteModel
            {
                Codigo_Flavorizante = CodigoFormulaTextBox.Text, // Código da Formula.
                Nome_Flavorizante = NomeFormulaTextBox.Text, // Nome da Formula.
                Data = DataFormulaPicker.Date.DateTime, // Data da Formula.
                Descricao_Processo = DescricaoFormulaTextBox.Text // Descrição do processo.
            };

            // Verifica se estamos editando uma formula existente ou criando uma nova.
            var formulaExistente = await _formulaFlavorizanteService.ObterFormulaPorCodigoAsync(formula.Codigo_Flavorizante);

            if (formulaExistente != null)
            {
                // **Editar formula existente (nova versão)**
                await CriarNovaVersaoAsync(formulaExistente.Id);
            }
            else
            {
                // **Criar nova formula**
                var formulaId = await _formulaFlavorizanteService.AdicionarFormulaAsync(formula);

                // Após criar uma nova formula, cria a primeira versão.
                await CriarNovaVersaoAsync(formulaId);
            }

            // Exibe mensagem de sucesso e limpa os campos.
            await MostrarDialogoAviso("Formula salva com sucesso!");
            LimparCampos();

            // Navega de volta para a página FormulaFlavorizanteView.
            Frame.Navigate(typeof(FormulaFlavorizanteView));
        }
        catch (Exception ex)
        {
            // Captura erros e exibe uma mensagem ao usuário.
            await MostrarDialogoAviso($"Erro ao salvar a formula: {ex.Message}");
        }
    }

    /// Cria uma nova versão para uma receita existente.
    /// <param name="formulaId">O ID da receita.</param>
    private async Task CriarNovaVersaoAsync(int formulaId)
    {
        try
        {
            // Obtém o número da última versão associada à receita.
            var ultimasVersoes = await _versoesFormulaFlavorizanteService.ObterPorFormulaIdAsync(formulaId);
            var novaVersaoNumero = (ultimasVersoes.Max(v => (int?)v.Versao) ?? 0) + 1;

            // Cria uma nova versão da receita.
            var versaoformula = new VersoesFormulaFlavorizanteModel
            {
                Id_Formula_Flavorizante = formulaId, // Relaciona a versão à receita.
                Versao = novaVersaoNumero, // Define o número da nova versão.
                Data = DateTime.Now, // Usa a data atual como data da versão.
                Descricao_Processo = DescricaoFormulaTextBox.Text // Usa a descrição informada.
            };

            // Adiciona a nova versão ao banco.
            var versaoFormulaId = await _versoesFormulaFlavorizanteService.AdicionarVersaoAsync(versaoformula);

            // Associa os insumos à nova versão.
            foreach (var item in ItensFormula.Where(i => !string.IsNullOrEmpty(i.Nome)))
            {
                var receitaInsumo = new FormulaFlavorizanteInsumosModel
                {
                    Id_Versao_Formula_Flavorizante = versaoFormulaId, // Relaciona ao ID da nova versão.
                    Id_Insumo = item.Idinsumo, // ID do insumo.
                    Unidade_Medida = item.UnidadeMedida, // Unidade de medida.
                    Quantidade = item.Quantidade, // Quantidade.
                    Id_Car = item.Idflavorizante, // ID do CAR.
                    
                };
                Debug.WriteLine($"Erro ao criar nova versão: {item.Quantidade}");

                // Salva o insumo no banco de dados.
                await _formulaFlavorizanteInsumosService.AdicionarAsync(receitaInsumo);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao criar nova versão: {ex.Message}");
            throw;
        }
    }





    private async void LimparCampos_Click(object sender, RoutedEventArgs e)
    {
        LimparCampos();
    }


    private async void LimparCampos()
    {
        // Limpa os campos de texto e reseta os controles
        NomeFormulaTextBox.Text = string.Empty;
        DescricaoFormulaTextBox.Text = string.Empty;
        DataFormulaPicker.Date = DateTimeOffset.Now;

        // Gera um novo código para a receita
        CodigoFormulaTextBox.Text = await GerarCodigoFormulaFlavorizanteAsync();

        // Limpa a lista de itens e adiciona uma linha vazia
        ItensFormula.Clear();
        AdicionarLinhaVazia();

        // Reseta o valor do peso total da amostra
        pesoTotalAmostra = 0;
    }

    public async Task<string> GerarCodigoFormulaFlavorizanteAsync()
    {
        int proximoId;
        try
        {
            proximoId = await _formulaFlavorizanteService.ObterProximoIdFormulaAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao obter próximo ID: {ex.Message}");
            return string.Empty;
        }

        if (proximoId <= 0)
        {
            Debug.WriteLine("Nenhum ID encontrado. Usando ID inicial 1.");
            proximoId = 1;
        }

        var diaDoAno = DateTime.Now.DayOfYear.ToString("000");
        var anoCorrente = (DateTime.Now.Year % 100).ToString("00");
        var codigoFormula = $"{diaDoAno}-{anoCorrente}-{proximoId:00}";

        Debug.WriteLine($"Código gerado: {codigoFormula}");
        return codigoFormula;
    }

    private void AtualizarGrafico()
    {
        // Limpa as séries existentes no gráfico
        PieSeries.Clear();

        // Adiciona os itens da receita ao gráfico
        foreach (var item in ItensFormula.Where(i => !string.IsNullOrWhiteSpace(i.Nome)))
        {
            PieSeries.Add(new PieSeries<double>
            {
                Name = item.Nome, // Nome do item no gráfico
                Values = new[] { (double)item.Quantidade }, // Conversão explícita de decimal para double
                MaxRadialColumnWidth = 50,
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle, // Rótulo no meio do gráfico
                DataLabelsPaint = new SolidColorPaint(SKColors.Black), // Cor dos rótulos
                DataLabelsFormatter = point => $"{item.Nome}: {point.Coordinate.PrimaryValue:F2}" // Exibe o nome e o valor Formatação percentual                                                                                                     
            });
        }

        // Notifica a interface sobre as mudanças
        OnPropertyChanged(nameof(PieSeries));
    }



    private async void FormulaFlavorizantePage_Loaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine($"Itens disponíveis: {ItensDisponiveis.Count}");
        if (_isNewFormula)
        {
            Debug.WriteLine($"Itens disponíveis: {ItensDisponiveis.Count}");

            // Gerar o código da receita ao carregar a página
            CodigoFormulaTextBox.Text = await GerarCodigoFormulaFlavorizanteAsync();

            // Converte a lista para ObservableCollection<ISeries>
            PieSeries = new ObservableCollection<ISeries>(
                ItensFormula.Select(item => new PieSeries<double>
                {
                    Values = new double[] { (double)item.Quantidade },
                    Name = item.Nome
                })
            );

            OnPropertyChanged(nameof(PieSeries));
        }
    }

    private void ReceitaListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ReceitaListView.SelectedItem is ItemModel selectedItem)
        {
            int index = ItensFormula.IndexOf(selectedItem);
            Debug.WriteLine($"Índice do item selecionado: {index}");
        }
    }



    private decimal _pesoTotalAmostra;
    public decimal pesoTotalAmostra
    {
        get => _pesoTotalAmostra;
        set
        {
            if (_pesoTotalAmostra != value)
            {
                _pesoTotalAmostra = value;
                OnPropertyChanged(nameof(pesoTotalAmostra));
            }
        }
    }

    private string _unidadeMedida = "g";
    public string UnidadeMedida
    {
        get => _unidadeMedida;
        set
        {
            if (_unidadeMedida != value)
            {
                _unidadeMedida = value;
                OnPropertyChanged(nameof(UnidadeMedida));
                AtualizarUnidadeMedidaNasLinhas();
            }
        }
    }

    private void AtualizarUnidadeMedidaNasLinhas()
    {
        foreach (var item in ItensFormula)
        {
            item.UnidadeMedida = UnidadeMedida;
        }

        // Atualizar interface do usuário
        DispatcherQueue.TryEnqueue(() =>
        {
            ReceitaListView.UpdateLayout();
        });
    }

    private void UnidadeMedidaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            // Verifica explicitamente se Content não é nulo
            UnidadeMedida = selectedItem.Content?.ToString() ?? string.Empty;
        }
        else
        {
            // Define um valor padrão caso nenhum item seja selecionado
            UnidadeMedida = string.Empty;
        }
    }


    private void QuantidadeTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Tab)
        {
            if (sender is TextBox textBox && textBox.DataContext is ItemModel currentItem)
            {
                // Localiza a posição atual na coleção
                var currentIndex = ItensFormula.IndexOf(currentItem);

                // Verifica se há uma próxima linha
                if (currentIndex >= 0 && currentIndex < ItensFormula.Count - 1)
                {
                    // Obtém o próximo item
                    var nextItem = ItensFormula[currentIndex + 1];

                    // Localiza o próximo TextBox correspondente (manualmente associado)
                    var nextContainer = FindVisualChild<TextBox>(ReceitaListView, currentIndex + 1, "QuantidadeTextBox");

                    if (nextContainer != null)
                    {
                        e.Handled = true; // Previne o comportamento padrão do TAB
                        nextContainer.Focus(FocusState.Programmatic);
                    }
                }
            }
        }
    }

    // Método auxiliar para localizar o TextBox em um índice específico
    private TextBox? FindVisualChild<T>(ListView listView, int index, string textBoxName) where T : DependencyObject
    {
        if (listView.ContainerFromIndex(index) is ListViewItem listViewItem)
        {
            // Busca recursiva pelo TextBox no ListViewItem
            return listViewItem.FindDescendant<TextBox>(tb => tb.Name == textBoxName);
        }
        return null;
    }


    private async Task CarregarItensDisponiveisAsync()
    {
        // Carregar insumos
        var insumos = await _insumoService.ObterTodosAsync();
        foreach (var insumo in insumos)
        {
            ItensDisponiveis.Add(new ItemModel
            {
                CodigoInterno = insumo.Codigo_Interno,
                Nome = insumo.Nome,
                Idinsumo = insumo.Id_Insumo

            });
        }

        // Carregar flavorizantes
        var flavorizantes = await _flavorizanteService.ObterTodosAsync();
        foreach (var flavorizante in flavorizantes)
        {
            ItensDisponiveis.Add(new ItemModel
            {
                CodigoInterno = flavorizante.Codigo_Interno,
                Nome = flavorizante.Nome,
                Idflavorizante = flavorizante.Id_Flavorizante
            });
        }

        // Carregar componentes Aromaticos
        var componentesAromaticos = await _componenteAromaticoService.ObterTodosAsync();
        foreach (var componenteAromatico in componentesAromaticos)
        {
            ItensDisponiveis.Add(new ItemModel
            {
               
                CodigoInterno = componenteAromatico.CodigoInterno,
                Nome = componenteAromatico.Nome,
                idcar = componenteAromatico.Id
            });            
        }

        // Atualizar layout no UI thread
        DispatcherQueue.TryEnqueue(() =>
        {
            ReceitaListView.UpdateLayout();
        });

        Debug.WriteLine($"Itens disponíveis carregados: {ItensDisponiveis.Count}");
    }

    private void AdicionarLinhaVazia()
    {
        var ultimaLinha = ItensFormula.LastOrDefault();
        if (ultimaLinha != null && string.IsNullOrEmpty(ultimaLinha.Nome))
        {
            Debug.WriteLine("A última linha já está vazia. Não será criada uma nova linha.");
            return;
        }

        // Adiciona uma nova linha vazia
        var novaLinha = new ItemModel
        {
            CodigoInterno = string.Empty,
            Nome = string.Empty,
            Quantidade = 0,
            UnidadeMedida = string.Empty,
            Porcentagem = 0
        };
        ItensFormula.Add(novaLinha);

        Debug.WriteLine("Nova linha vazia adicionada.");

        // Focar o campo "Nome" da nova linha
        DispatcherQueue.TryEnqueue(() =>
        {
            var container = ReceitaListView.ContainerFromItem(novaLinha) as ListViewItem;
            if (container != null)
            {
                var autoSuggestBox = container.FindDescendant<AutoSuggestBox>();
                autoSuggestBox?.Focus(FocusState.Programmatic);
            }
        });
    }

    private void OnAutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suggestions = ItensDisponiveis
                .Where(i => i.Nome.Contains(sender.Text, StringComparison.OrdinalIgnoreCase) ||
                            i.CodigoInterno.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                .ToList();

            sender.ItemsSource = suggestions;
        }
    }

    private void OnAutoSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is ItemModel selectedItem)
        {
            AdicionarItemSelecionado(selectedItem, sender);
        }
    }

    private void AdicionarItemSelecionado(ItemModel selectedItem, AutoSuggestBox sender)
    {
        // Verifica a linha atual vinculada ao AutoSuggestBox
        if (sender.DataContext is ItemModel currentItem)
        {
            // Preenche a linha atual com os dados do item selecionado
            currentItem.CodigoInterno = selectedItem.CodigoInterno;
            currentItem.Nome = selectedItem.Nome;
            currentItem.UnidadeMedida = "g"; // Unidade padrão ou conforme necessidade
            currentItem.Quantidade = 0; // Inicializa para edição
            currentItem.Porcentagem = 0; // Inicializa para edição
            currentItem.Idflavorizante = selectedItem.Idflavorizante;
            currentItem.IdflavorizanteInterno = selectedItem.IdflavorizanteInterno;
            currentItem.Idinsumo = selectedItem.Idinsumo;
            // Atualiza a interface para refletir as alterações
            DispatcherQueue.TryEnqueue(() =>
            {
                ReceitaListView.UpdateLayout();
            });

            Debug.WriteLine($"Linha preenchida: Código Interno: {currentItem.CodigoInterno}, Nome: {currentItem.Nome}, INSUMO: {selectedItem.Idinsumo}, FLAVORIZANTE: {selectedItem.Idflavorizante}");
        }

        // Verifica se a última linha está vazia antes de adicionar outra
        var ultimaLinha = ItensFormula.LastOrDefault();
        if (ultimaLinha != null && string.IsNullOrEmpty(ultimaLinha.Nome))
        {
            Debug.WriteLine("Linha vazia já existe. Não será adicionada uma nova linha.");
            return;
        }

        // Adiciona uma nova linha para entrada de um próximo item
        AdicionarLinhaVazia();

        // Foco no AutoSuggestBox da nova linha
        var novaLinha = ItensFormula.LastOrDefault();
        if (novaLinha != null)
        {
            var container = ReceitaListView.ContainerFromItem(novaLinha) as ListViewItem;
            if (container != null)
            {
                var autoSuggestBox = container.FindDescendant<AutoSuggestBox>();
                autoSuggestBox?.Focus(FocusState.Programmatic);
            }
        }
    }

    private void QuantidadeTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox && textBox.DataContext is ItemModel currentItem)
        {
            // Remove qualquer caractere inválido
            string cleanText = new string(textBox.Text.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());

            // Substitui vírgula por ponto (se necessário)
            cleanText = cleanText.Replace(',', '.');

            // Tenta converter para decimal e formatar
            if (decimal.TryParse(cleanText, out decimal value))
            {
                currentItem.Quantidade = value; // Atualiza a quantidade do modelo
                textBox.Text = value.ToString("0.00"); // Atualiza o texto no TextBox
            }
            else
            {
                currentItem.Quantidade = 0; // Valor padrão para entradas inválidas
                textBox.Text = "0.00";
            }

            // Atualiza as porcentagens com base na nova quantidade
            AtualizarPorcentagens();
            AtualizarGrafico();
        }
    }

    private void PorcentagemTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox && textBox.DataContext is ItemModel currentItem)
        {
            const decimal totalPorcentagem = 100m; // Total de porcentagem (100%)

            // Obter todos os itens válidos (ignorando linhas sem nome)
            var itensValidos = ItensFormula.Where(item => !string.IsNullOrEmpty(item.Nome)).ToList();

            // Verificar se o item atual está nos itens válidos
            if (!itensValidos.Contains(currentItem))
            {
                return; // Ignorar se o item atual não for válido
            }

            // Tratamento para entrada inválida
            string input = textBox.Text.Trim();

            // Substituir vírgulas por pontos e remover caracteres não numéricos
            input = new string(input.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray())
                         .Replace(',', '.'); // Substitui ',' por '.' para culturas com vírgula como separador

            // Capturar o valor da porcentagem do campo editado
            if (!decimal.TryParse(input, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out decimal novaPorcentagem))
            {
                Debug.WriteLine("Erro: Valor de porcentagem inválido.");
                textBox.Text = currentItem.Porcentagem.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); // Restaura o valor anterior
                return;
            }

            // Limitar a porcentagem entre 0 e 100
            novaPorcentagem = Math.Clamp(novaPorcentagem, 0, totalPorcentagem);

            // Calcula a diferença entre a porcentagem atual e a nova
            decimal diferenca = currentItem.Porcentagem - novaPorcentagem;

            // Atualiza a porcentagem do item ajustado
            currentItem.Porcentagem = novaPorcentagem;

            if (Math.Abs(diferenca) > 0)
            {
                // Soma dos pesos sem o item ajustado
                decimal pesoTotalSemAjustado = itensValidos
                    .Where(item => item != currentItem)
                    .Sum(item => item.Porcentagem);

                // Redistribuir a diferença proporcionalmente entre os outros itens
                foreach (var item in itensValidos.Where(item => item != currentItem))
                {
                    // Ajuste proporcional para cada item
                    decimal ajuste = diferenca * (item.Porcentagem / pesoTotalSemAjustado);
                    item.Porcentagem += ajuste;
                }

                // Garantir que a soma seja exatamente 100%
                decimal somaPorcentagens = itensValidos.Sum(item => item.Porcentagem);
                decimal ajusteFinal = totalPorcentagem - somaPorcentagens;

                // Aplica ajuste final ao primeiro item válido, exceto o ajustado
                var primeiroItem = itensValidos.FirstOrDefault(item => item != currentItem);
                if (primeiroItem != null)
                {
                    primeiroItem.Porcentagem += ajusteFinal;
                }
            }

            // Recalcular todas as quantidades com base nas porcentagens atualizadas
            AtualizarQuantidades(itensValidos);

            // Atualiza o gráfico
            AtualizarGrafico();
        }
    }



    private void AtualizarQuantidades(IEnumerable<ItemModel> itensValidos)
    {
        if (pesoTotalAmostra > 0)
        {
            foreach (var item in itensValidos)
            {
                item.Quantidade = Math.Round((pesoTotalAmostra * item.Porcentagem) / 100, 2);
            }
        }
        else
        {
            foreach (var item in itensValidos)
            {
                item.Quantidade = 0; // Evita divisão por zero
            }
        }

        // Atualiza a interface do usuário
        DispatcherQueue.TryEnqueue(() =>
        {
            ReceitaListView.UpdateLayout();
        });
    }

    private void PorcentagemTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Selecionar todo o texto quando o campo ganha foco
            textBox.SelectAll();
        }
    }

    private void AtualizarPorcentagens()
    {
        // Obtém apenas os itens válidos (ignorando linhas sem nome).
        var itensValidos = ItensFormula.Where(item => !string.IsNullOrEmpty(item.Nome)).ToList();

        // Calcula o total de quantidades de todos os itens válidos.
        decimal totalQuantidade = itensValidos.Sum(item => item.Quantidade);

        // Verifica se o total de quantidades é maior que zero.
        if (totalQuantidade > 0)
        {
            // Atualiza a porcentagem de cada item com base na quantidade total.
            foreach (var item in itensValidos)
            {
                // Calcula a porcentagem como (quantidade / total) * 100 e arredonda para 3 casas decimais.
                item.Porcentagem = Math.Round((item.Quantidade / totalQuantidade) * 100, 3);
            }

            // Calcula a soma atual das porcentagens para verificar se há ajustes necessários.
            decimal somaPorcentagens = itensValidos.Sum(item => item.Porcentagem);

            // Calcula a diferença entre 100% e a soma atual.
            decimal diferenca = 100m - somaPorcentagens;

            // Caso a soma não seja exatamente 100%, ajusta a diferença no primeiro item.
            if (diferenca != 0)
            {
                var primeiroItem = itensValidos.FirstOrDefault();
                if (primeiroItem != null)
                {
                    // Adiciona a diferença ao primeiro item para garantir que a soma seja 100%.
                    primeiroItem.Porcentagem += diferenca;
                }
            }
        }
        else
        {
            // Caso o total de quantidades seja zero, zera as porcentagens de todos os itens.
            foreach (var item in itensValidos)
            {
                item.Porcentagem = 0;
            }
        }

        // Atualiza o layout da interface do usuário para refletir as alterações.
        DispatcherQueue.TryEnqueue(() =>
        {
            ReceitaListView.UpdateLayout();
        });

        // Atualiza o valor total da amostra para refletir o total de quantidades.
        pesoTotalAmostra = totalQuantidade;

        // Notifica a interface sobre a mudança no peso total da amostra.
        OnPropertyChanged(nameof(pesoTotalAmostra));
    }




    private void OnAutoSuggestBoxSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is ItemModel selectedItem)
        {
            AdicionarItemSelecionado(selectedItem, sender);
        }
    }

    // Sobrecarga para compatibilidade com chamadas antigas
    private void AtualizarQuantidades()
    {
        AtualizarQuantidades(ItensFormula.Where(item => !string.IsNullOrEmpty(item.Nome)));
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Comportamento 1: Selecionar todo o texto
            textBox.SelectAll();

            // OU

            // Comportamento 2: Limpar o texto ao focar
            // textBox.Text = string.Empty;
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
}
