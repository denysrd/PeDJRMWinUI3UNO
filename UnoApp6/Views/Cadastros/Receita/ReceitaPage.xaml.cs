using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using PeDJRMWinUI3UNO.Services;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System.ComponentModel;
using Uno.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Receita
{
    public sealed partial class ReceitaPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly InsumosService _insumoService;
        private readonly FlavorizantesService _flavorizanteService;
        private readonly ReceitasService _receitasService;

        public ObservableCollection<InsumosModel> Insumos { get; set; } = new ObservableCollection<InsumosModel>();
        public ObservableCollection<FlavorizantesModel> Flavorizantes { get; set; } = new ObservableCollection<FlavorizantesModel>();
        public ObservableCollection<ReceitasModel> Receitas { get; set; } = new ObservableCollection<ReceitasModel>();

        public ObservableCollection<ItemModel> ItensReceita { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> ItensDisponiveis { get; set; } = new ObservableCollection<ItemModel>();

        public ReceitaPage()
        {
            this.InitializeComponent();
            this.DataContext = this; // Configura o DataContext para a própria página
            _insumoService = (InsumosService?)App.Services.GetService(typeof(InsumosService));
            _flavorizanteService = (FlavorizantesService)App.Services.GetService(typeof(FlavorizantesService));
            _receitasService = (ReceitasService)App.Services.GetService(typeof(ReceitasService));

            if (_insumoService == null || _flavorizanteService == null || _receitasService == null)
            {
                MostrarDialogoAviso("Erro ao carregar os serviços. Verifique a configuração do serviço.");
                return;
            }

            // Carregar itens disponíveis e inicializar a primeira linha
            _ = CarregarItensDisponiveisAsync();
            AdicionarLinhaVazia();           
        }

        public async Task<string> GerarCodigoReceitaAsync()
        {           
            int proximoId;
            try
            {
                proximoId = await _receitasService.ObterProximoIdReceitaAsync();
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
            var codigoReceita = $"{diaDoAno}-{anoCorrente}-{proximoId:00}";

            Debug.WriteLine($"Código gerado: {codigoReceita}");
            return codigoReceita;
        }





        private async void ReceitaPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Itens disponíveis: {ItensDisponiveis.Count}");

            // Gerar o código da receita ao carregar a página
            CodigoReceitaTextBox.Text = await GerarCodigoReceitaAsync();
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
            foreach (var item in ItensReceita)
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
                UnidadeMedida = selectedItem.Content.ToString();
            }
        }

        private void QuantidadeTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                if (sender is TextBox textBox && textBox.DataContext is ItemModel currentItem)
                {
                    // Localiza a posição atual na coleção
                    var currentIndex = ItensReceita.IndexOf(currentItem);

                    // Verifica se há uma próxima linha
                    if (currentIndex >= 0 && currentIndex < ItensReceita.Count - 1)
                    {
                        // Obtém o próximo item
                        var nextItem = ItensReceita[currentIndex + 1];

                        // Localiza o TextBox correspondente na próxima linha
                        var container = ReceitaListView.ContainerFromItem(nextItem) as ListViewItem;
                        if (container != null)
                        {
                            var nextTextBox = container.FindDescendant<TextBox>(tb => tb.Name == "QuantidadeTextBox");
                            if (nextTextBox != null)
                            {
                                e.Handled = true; // Previne o comportamento padrão do TAB
                                nextTextBox.Focus(FocusState.Programmatic);
                            }
                        }
                    }
                }
            }
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
                    Nome = insumo.Nome
                });
            }

            // Carregar flavorizantes
            var flavorizantes = await _flavorizanteService.ObterTodosAsync();
            foreach (var flavorizante in flavorizantes)
            {
                ItensDisponiveis.Add(new ItemModel
                {
                    CodigoInterno = flavorizante.Codigo_Interno,
                    Nome = flavorizante.Nome
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
            var ultimaLinha = ItensReceita.LastOrDefault();
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
            ItensReceita.Add(novaLinha);

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

                // Atualiza a interface para refletir as alterações
                DispatcherQueue.TryEnqueue(() =>
                {
                    ReceitaListView.UpdateLayout();
                });

                Debug.WriteLine($"Linha preenchida: Código Interno: {currentItem.CodigoInterno}, Nome: {currentItem.Nome}");
            }

            // Verifica se a última linha está vazia antes de adicionar outra
            var ultimaLinha = ItensReceita.LastOrDefault();
            if (ultimaLinha != null && string.IsNullOrEmpty(ultimaLinha.Nome))
            {
                Debug.WriteLine("Linha vazia já existe. Não será adicionada uma nova linha.");
                return;
            }

            // Adiciona uma nova linha para entrada de um próximo item
            AdicionarLinhaVazia();

            // Foco no AutoSuggestBox da nova linha
            var novaLinha = ItensReceita.LastOrDefault();
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
            }
        }

        private void PorcentagemTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is ItemModel currentItem)
            {
                const decimal totalPorcentagem = 100m; // Total de porcentagem (100%)

                // Obter todos os itens válidos (ignorando linhas sem nome)
                var itensValidos = ItensReceita.Where(item => !string.IsNullOrEmpty(item.Nome)).ToList();

                // Verificar se o item atual está nos itens válidos
                if (!itensValidos.Contains(currentItem))
                {
                    return; // Ignorar se o item atual não for válido
                }

                // Capturar o valor da porcentagem do campo editado
                if (decimal.TryParse(textBox.Text.Replace(",", "."), out decimal novaPorcentagem))
                {
                    currentItem.Porcentagem = novaPorcentagem;
                }
                else
                {
                    Debug.WriteLine("Erro: Valor de porcentagem inválido.");
                    return;
                }

                // Soma das porcentagens restantes
                decimal restantePorcentagem = totalPorcentagem - currentItem.Porcentagem;

                if (restantePorcentagem < 0)
                {
                    Debug.WriteLine("Erro: Porcentagem total não pode exceder 100%.");
                    return;
                }

                // Redistribuir o restante proporcionalmente entre os outros itens
                var outrosItens = itensValidos.Where(item => item != currentItem).ToList();
                if (outrosItens.Any())
                {
                    decimal porcentagemPorItem = Math.Round(restantePorcentagem / outrosItens.Count, 2);

                    foreach (var item in outrosItens)
                    {
                        item.Porcentagem = porcentagemPorItem;
                    }

                    // Ajustar a soma final para garantir que seja exatamente 100%
                    decimal somaAtual = itensValidos.Sum(item => item.Porcentagem);
                    decimal diferenca = totalPorcentagem - somaAtual;
                    if (diferenca != 0)
                    {
                        var primeiroItem = outrosItens.FirstOrDefault();
                        if (primeiroItem != null)
                        {
                            primeiroItem.Porcentagem += diferenca;
                        }
                    }
                }

                // Recalcular todas as quantidades com base nas porcentagens atualizadas
                AtualizarQuantidades(itensValidos);
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
            // Obter itens válidos (ignorando linhas sem nome)
            var itensValidos = ItensReceita.Where(item => !string.IsNullOrEmpty(item.Nome)).ToList();

            // Calcula o total de quantidades
            decimal totalQuantidade = itensValidos.Sum(item => item.Quantidade);

            if (totalQuantidade > 0)
            {
                foreach (var item in itensValidos)
                {
                    // Calcula a porcentagem com base na quantidade
                    item.Porcentagem = Math.Round((item.Quantidade / totalQuantidade) * 100, 2);
                }

                // Ajuste final para garantir que a soma seja exatamente 100%
                decimal somaPorcentagens = itensValidos.Sum(item => item.Porcentagem);
                decimal diferenca = 100m - somaPorcentagens;
                if (diferenca != 0)
                {
                    var primeiroItem = itensValidos.FirstOrDefault();
                    if (primeiroItem != null)
                    {
                        primeiroItem.Porcentagem += diferenca;
                    }
                }
            }
            else
            {
                // Zera as porcentagens se o total for zero
                foreach (var item in itensValidos)
                {
                    item.Porcentagem = 0;
                }
            }

            // Atualiza a interface
            DispatcherQueue.TryEnqueue(() =>
            {
                ReceitaListView.UpdateLayout();
            });

            // Atualiza o valor total da amostra
            pesoTotalAmostra = totalQuantidade;
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
            AtualizarQuantidades(ItensReceita.Where(item => !string.IsNullOrEmpty(item.Nome)));
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

    public static class UIElementExtensions
    {
        public static T? FindDescendant<T>(this DependencyObject element) where T : DependencyObject
        {
            if (element == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is T descendant)
                {
                    return descendant;
                }

                var result = FindDescendant<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}

public static class UIElementExtensions
{
    public static T? FindDescendant<T>(this DependencyObject element, Func<T, bool>? predicate = null) where T : DependencyObject
    {
        if (element == null) return null;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var child = VisualTreeHelper.GetChild(element, i);
            if (child is T descendant && (predicate == null || predicate(descendant)))
            {
                return descendant;
            }

            var result = FindDescendant<T>(child, predicate);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
