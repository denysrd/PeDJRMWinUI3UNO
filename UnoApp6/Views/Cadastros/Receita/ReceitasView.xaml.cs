using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System;

namespace PeDJRMWinUI3UNO.Views.Cadastros.Receita;

/// Define a lógica da página ReceitasView
public sealed partial class ReceitasView : Page
{
    // Coleção para armazenar receitas carregadas
    public ObservableCollection<ReceitasModel> Receitas { get; set; } = new ObservableCollection<ReceitasModel>();
    // Coleções para os itens da receita e itens disponíveis.
    public ObservableCollection<ItemModel> ItensReceita { get; set; } = new ObservableCollection<ItemModel>();
    // Coleções para os itens da receita e itens disponíveis.
    public ObservableCollection<ReceitasInsumosModel> ReceitasInsumosModels { get; set; } = new ObservableCollection<ReceitasInsumosModel>();

    // Serviços
    private readonly ReceitasService _receitasService;
    private readonly ReceitasInsumosService _receitasInsumoService;

    public ReceitasView()
    {
        // Inicializa os componentes da página
        this.InitializeComponent();
        // Define o DataContext para esta classe
        this.DataContext = this;

        // Obtém o serviço de receitas do contêiner de injeção de dependência
        _receitasService = App.Services.GetService<ReceitasService>()
            ?? throw new InvalidOperationException("O serviço ReceitasService não foi encontrado.");
        // Obtém o serviço de receitas do contêiner de injeção de dependência
        _receitasInsumoService = App.Services.GetService<ReceitasInsumosService>()
            ?? throw new InvalidOperationException("O serviço ReceitasInsumosService não foi encontrado.");


        // Carrega as receitas ao inicializar
        _ = CarregarReceitasAsync();
    }



    private async void VersaoExpanding(object sender, object e)
    {
        // Adiciona um atraso de 500ms antes de executar o restante da lógica
        await Task.Delay(500);
        try
            {
            // Verifica se o sender é um Expander e se o DataContext está correto
            if (sender is Expander expander && expander.Tag is VersoesReceitasModel versao)
            {                
                // Verifica se os itens já foram carregados para evitar recarregar
                if (versao.Itens != null && versao.Itens.Any())
                    {
                        Debug.WriteLine($"Itens já carregados para a versão {versao.Versao}. Ignorando recarregamento.");
                        return;
                    }
                Debug.WriteLine($"Itens carregados para a versão {versao.Versao}: {versao.Itens?.Count ?? 0}");

                // Busca os insumos da versão pelo serviço
                var receitasInsumos = await _receitasInsumoService.ObterPorVersaoReceitaIdAsync(versao.Id);

                    // Mapeia os insumos para o modelo ItemModel
                    var itens = receitasInsumos.Select(insumo => new ItemModel
                    {
                        CodigoInterno = insumo.Insumo?.Codigo_Interno ?? insumo.Flavorizante?.Codigo_Interno ?? "N/A",
                        Nome = insumo.Insumo?.Nome ?? insumo.Flavorizante?.Nome ?? "Sem nome",
                        Quantidade = insumo.Quantidade,                      
                        Idinsumo = insumo.Id_Insumo ?? 0, // Atribui 0 se for nulo
                        Idflavorizante = insumo.Id_Flavorizante ?? 0 // Atribui 0 se for nulo
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

                // Para cada versão, carregar os itens associados
                foreach (var versao in versoes)
                {
                    try
                    {
                        // Busca os insumos da versão pelo serviço
                        var receitasInsumos = await _receitasInsumoService.ObterPorVersaoReceitaIdAsync(versao.Id);

                        // Mapeia os insumos para o modelo ItemModel
                        var itens = receitasInsumos.Select(insumo => new ItemModel
                        {
                            CodigoInterno = insumo.Insumo?.Codigo_Interno ?? insumo.Flavorizante?.Codigo_Interno ?? "N/A",
                            Nome = insumo.Insumo?.Nome ?? insumo.Flavorizante?.Nome ?? "Sem nome",
                            Quantidade = insumo.Quantidade,
                            Idinsumo = insumo.Id_Insumo ?? 0, // Atribui 0 se for nulo
                            Idflavorizante = insumo.Id_Flavorizante ?? 0, // Atribui 0 se for nulo
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

    

    /// Edita uma receita ou versão selecionada
    private void OnEditButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is VersoesReceitasModel versaoParaEditar)
        {
            Frame.Navigate(typeof(ReceitaPage), versaoParaEditar);
        }
    }

    /// Exclui uma receita inteira
    private async void ExcluirReceitaCompleta_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is ReceitasModel receitaParaExcluir)
        {
            // Confirmar exclusão
            var dialog = new ContentDialog
            {
                Title = "Excluir Receita",
                Content = $"Tem certeza de que deseja excluir a receita '{receitaParaExcluir.Nome_Receita}' e todas as suas versões?",
                PrimaryButtonText = "Excluir",
                CloseButtonText = "Cancelar",
                XamlRoot = this.XamlRoot
            };

            var resultado = await dialog.ShowAsync();
            if (resultado == ContentDialogResult.Primary)
            {
                try
                {
                    // Remove a receita utilizando o serviço
                    await _receitasService.RemoverReceitaComVersoesAsync(receitaParaExcluir.Id);

                    // Recarrega as receitas após a exclusão
                    await CarregarReceitasAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao excluir receita: {ex.Message}");
                    await MostrarDialogoAviso($"Erro ao excluir receita: {ex.Message}");
                }
            }
        }
    }



    private void OnCopiarButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is VersoesReceitasModel versaoSelecionada)
        {
            // Navega para a página ReceitaPage com os dados da versão a ser copiada
            CopiarReceita(versaoSelecionada);
        }
    }

    private void CopiarReceita(VersoesReceitasModel versaoSelecionada)
    {
        if (versaoSelecionada == null)
        {
            Debug.WriteLine("Nenhuma versão selecionada para copiar.");
            return;
        }

        try
        {
            // Navega para ReceitaPage, sinalizando que é uma cópia
            var parametrosCopia = new ReceitasModel
            {
                Id = versaoSelecionada.Id_Receita,
                Nome_Receita = string.Empty, // Nome será inserido pelo usuário
                Codigo_Receita = string.Empty, // Gerará um novo código
                Data = DateTime.Now, // Data atual para a cópia
                Descricao_Processo = string.Empty // Limpa descrição para nova entrada
            };

            Frame.Navigate(typeof(ReceitaPage), parametrosCopia);
            Debug.WriteLine($"Navegando para copiar a receita com ID {versaoSelecionada.Id_Receita}.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao copiar receita: {ex.Message}");
        }
    }



    /// Exclui a receita ou versão selecionada
    private async void ExcluirVersao_Click(object sender, RoutedEventArgs e)
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


}
