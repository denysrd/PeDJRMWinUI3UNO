using Microsoft.Extensions.DependencyInjection;
using PeDJRMWinUI3UNO.Services;
using PeDJRMWinUI3UNO.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PeDJRMWinUI3UNO.Views.Cadastros
{
    public sealed partial class FlavorizantesView : Page
    {
        private readonly FlavorizantesService _flavorizantesService;
        private readonly FornecedorService _fornecedorService;
        private readonly TipoIngredienteService _tipoIngredienteService;

        private int _itensPorPagina = 10;
        private int _paginaAtual = 1;
        private ObservableCollection<FlavorizantesModel> _todosFlavorizantes = new ObservableCollection<FlavorizantesModel>();

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; } = new ObservableCollection<FornecedorModel>();
        public ObservableCollection<TipoIngredienteModel> TipoAromas { get; set; } = new ObservableCollection<TipoIngredienteModel>();
        public ObservableCollection<FlavorizantesModel> Flavorizantes { get; set; } = new ObservableCollection<FlavorizantesModel>();

        public bool TemPaginaAnterior => _paginaAtual > 1;
        public bool TemPaginaProxima => _paginaAtual < NumeroTotalPaginas;
        public int NumeroTotalPaginas => (_todosFlavorizantes.Count + _itensPorPagina - 1) / _itensPorPagina; // Cálculo do total de páginas

        public FlavorizantesView()
        {
            this.InitializeComponent();
            _flavorizantesService = App.Services.GetService<FlavorizantesService>();
            _fornecedorService = App.Services.GetService<FornecedorService>();
            _tipoIngredienteService = App.Services.GetService<TipoIngredienteService>();

            CarregarDadosAsync();
        }

        private async void CarregarDadosAsync()
        {
            // Carregar fornecedores
            Fornecedores.Clear();
            var fornecedores = await _fornecedorService.ObterTodosFornecedoresAsync();
            foreach (var fornecedor in fornecedores)
            {
                Fornecedores.Add(fornecedor);
            }
            FornecedorComboBox.ItemsSource = Fornecedores;

            // Carregar tipos de aroma filtrados (FLA e FLI)
            TipoAromas.Clear();
            var tiposAroma = await _tipoIngredienteService.ObterTodosAsync();
            var tiposFiltrados = tiposAroma.Where(t => t.Sigla == "FLA" || t.Sigla == "FLI");
            foreach (var tipo in tiposFiltrados)
            {
                TipoAromas.Add(tipo);
            }
            TipoAromaComboBox.ItemsSource = TipoAromas;

            // Carregar flavorizantes
            _todosFlavorizantes.Clear();
            var flavorizantes = await _flavorizantesService.ObterTodosAsync();
            foreach (var flavorizante in flavorizantes)
            {
                _todosFlavorizantes.Add(flavorizante);
            }
            AtualizarPaginacao();
        }

        private void AtualizarPaginacao()
        {
            Flavorizantes.Clear();
            var itemsFiltrados = _todosFlavorizantes
                .Where(f => Filtro(f))
                .Skip((_paginaAtual - 1) * _itensPorPagina)
                .Take(_itensPorPagina)
                .ToList();
            foreach (var item in itemsFiltrados)
            {
                Flavorizantes.Add(item);
            }
            FlavorizanteDataGrid.ItemsSource = Flavorizantes;

            // Atualizar estado dos botões de paginação e o número de página exibido
            this.Bindings.Update();
        }

        private bool Filtro(FlavorizantesModel flavorizante)
        {
            var termo = FiltroTextBox.Text?.ToLower() ?? "";
            return flavorizante.Nome.ToLower().Contains(termo) ||
                   flavorizante.Custo.ToString().Contains(termo) ||
                   flavorizante.Codigo_Interno.ToLower().Contains(termo) ||
                   flavorizante.Codigo_Fornecedor.ToLower().Contains(termo);
        }

        private void FiltroTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _paginaAtual = 1;  // Reiniciar para a primeira página ao filtrar
            AtualizarPaginacao();
        }

        private void ItensPorPaginaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItensPorPaginaComboBox.SelectedItem is ComboBoxItem item)
            {
                _itensPorPagina = item.Content.ToString() == "Todos" ? _todosFlavorizantes.Count : int.Parse(item.Content.ToString());
                _paginaAtual = 1;  // Reiniciar para a primeira página
                AtualizarPaginacao();
            }
        }

        private void PaginaAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (TemPaginaAnterior)
            {
                _paginaAtual--;
                AtualizarPaginacao();
            }
        }

        private void PaginaProxima_Click(object sender, RoutedEventArgs e)
        {
            if (TemPaginaProxima)
            {
                _paginaAtual++;
                AtualizarPaginacao();
            }
        }

        private async void SalvarFlavorizante_Click(object sender, RoutedEventArgs e)
        {
            if (TipoAromaComboBox.SelectedItem is TipoIngredienteModel tipoAroma && FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
            {
                // Geração do Código Interno
                int proximoId = await _flavorizantesService.ObterProximoIdAsync();
                CodigoInternoTextBox.Text = $"{tipoAroma.Sigla}-{proximoId}-{fornecedor.Id_Fornecedor}";

                // Criação do novo flavorizante
                var novoFlavorizante = new FlavorizantesModel
                {
                    Nome = NomeTextBox.Text,
                    Custo = decimal.TryParse(CustoTextBox.Text, out var custo) ? custo : 0,
                    Codigo_Interno = CodigoInternoTextBox.Text,
                    Codigo_Fornecedor = CodigoFornecedorTextBox.Text,
                    Id_Fornecedor = fornecedor.Id_Fornecedor,
                    Situacao = SituacaoToggleSwitch.IsOn
                };

                await _flavorizantesService.SalvarAsync(novoFlavorizante);
                _todosFlavorizantes.Add(novoFlavorizante);
                AtualizarPaginacao();
                LimparCampos();
            }
            else
            {
                await MostrarDialogoAviso("Por favor, selecione o tipo de aroma e o fornecedor.");
            }
        }

        private void LimparCampos_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            NomeTextBox.Text = string.Empty;
            CustoTextBox.Text = string.Empty;
            CodigoInternoTextBox.Text = string.Empty;
            CodigoFornecedorTextBox.Text = string.Empty;
            TipoAromaComboBox.SelectedIndex = -1;
            FornecedorComboBox.SelectedIndex = -1;
            SituacaoToggleSwitch.IsOn = false;
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

        // Eventos para atualizar o código interno automaticamente
        private async void TipoAromaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await AtualizarCodigoInterno();
        }

        private async void FornecedorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await AtualizarCodigoInterno();
        }

        private async Task AtualizarCodigoInterno()
        {
            if (TipoAromaComboBox.SelectedItem is TipoIngredienteModel tipoAroma && FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
            {
                int proximoId = await _flavorizantesService.ObterProximoIdAsync();
                CodigoInternoTextBox.Text = $"{tipoAroma.Sigla}-{proximoId}-{fornecedor.Id_Fornecedor}";
            }
            else
            {
                CodigoInternoTextBox.Text = string.Empty;
            }
        }
    }
}
