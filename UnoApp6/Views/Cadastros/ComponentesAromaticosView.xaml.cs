using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PeDJRMWinUI3UNO.Services;

namespace PeDJRMWinUI3UNO.Views.Cadastros
{
    public sealed partial class ComponentesAromaticosView : Page
    {
        private readonly ComponenteAromaticoService _componenteAromaticoService;
        private readonly FornecedorService _fornecedorService;

        private ComponenteAromaticoModel componenteAromaticoEmEdicao;

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; } = new ObservableCollection<FornecedorModel>();
        public ObservableCollection<ComponenteAromaticoModel> ComponentesAromaticos { get; set; } = new ObservableCollection<ComponenteAromaticoModel>();

        public ComponentesAromaticosView()
        {
            this.InitializeComponent();

            _componenteAromaticoService = App.Services.GetService<ComponenteAromaticoService>();
            _fornecedorService = App.Services.GetService<FornecedorService>();

            if (_componenteAromaticoService == null || _fornecedorService == null)
            {
                MostrarDialogoAviso("Erro ao carregar os serviços. Verifique a configuração do serviço.");
                return;
            }

            CarregarDadosAsync();
        }

        private async void CarregarDadosAsync()
        {
            Fornecedores.Clear();

            // Carregar fornecedores
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

            CarregarRegistrosAsync();
        }

        private async void CarregarRegistrosAsync()
        {
            // Recria a coleção de insumos
            ComponentesAromaticos = new ObservableCollection<ComponenteAromaticoModel>(await _componenteAromaticoService.ObterTodosAsync());
            ComponentesAromaticosDataGrid.ItemsSource = ComponentesAromaticos;
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
            // Define valores fixos para a sigla e Id_Tipo_Ingrediente
            string sigla = "CAR";
            string idFornecedor = "";

            if (FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
            {
                idFornecedor = fornecedor.Id_Fornecedor.ToString();
            }

            // Obtém o próximo ID para criação de um novo código
            int proximoId = await _componenteAromaticoService.ObterProximoIdAsync();
            CodigoInternoTextBox.Text = $"{sigla}-{proximoId}-{idFornecedor}";
        }

        private async void SalvarInsumo_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeTextBox.Text) || FornecedorComboBox.SelectedItem == null)
            {
                await MostrarDialogoAviso("Preencha todos os campos obrigatórios.");
                return;
            }

            if (FornecedorComboBox.SelectedItem is FornecedorModel fornecedor)
            {
                bool novoRegistro = componenteAromaticoEmEdicao == null;

                if (novoRegistro)
                {
                    // Criando novo registro
                    componenteAromaticoEmEdicao = new ComponenteAromaticoModel
                    {
                        Id = await _componenteAromaticoService.ObterProximoIdAsync(),
                        CodigoInterno = CodigoInternoTextBox.Text
                    };
                }
                else
                {
                    // Atualiza o Código Interno antes de salvar, para que o valor correto seja salvo
                    componenteAromaticoEmEdicao.CodigoInterno = CodigoInternoTextBox.Text;
                }

                // Atualizando os campos do insumo com as novas informações
                componenteAromaticoEmEdicao.Nome = NomeTextBox.Text;
                componenteAromaticoEmEdicao.NomenclaturaEn = NomeEnTextBox.Text;
                componenteAromaticoEmEdicao.LinkReferencia = LinkTextBox.Text;
                componenteAromaticoEmEdicao.CAS = CasTextBox.Text;
                componenteAromaticoEmEdicao.Fema = FemaTextBox.Text;
                componenteAromaticoEmEdicao.Custo = decimal.TryParse(CustoTextBox.Text, out var custo) ? custo : 0;
                componenteAromaticoEmEdicao.Descricao = DescricaoTextBox.Text;
                componenteAromaticoEmEdicao.CodigoProdutoFornecedor = CodigoFornecedorTextBox.Text;
                componenteAromaticoEmEdicao.IdFornecedor = fornecedor.Id_Fornecedor;
                componenteAromaticoEmEdicao.IdTipoIngrediente = 6; // Define sempre 6
                componenteAromaticoEmEdicao.Situacao = SituacaoToggleSwitch.IsOn;
                DataRecebimentoPicker.Date = DateTimeOffset.Now;
                DataCadastroPicker.Date = DateTimeOffset.Now;

                bool sucesso;
                if (novoRegistro) // Novo insumo
                {
                    sucesso = await _componenteAromaticoService.SalvarAsync(componenteAromaticoEmEdicao);
                    if (sucesso)
                    {
                        ComponentesAromaticos.Add(componenteAromaticoEmEdicao);
                    }
                }
                else // Atualização de insumo existente
                {
                    sucesso = await _componenteAromaticoService.AtualizarAsync(componenteAromaticoEmEdicao);
                    if (sucesso)
                    {
                        var index = ComponentesAromaticos.IndexOf(componenteAromaticoEmEdicao);
                        if (index >= 0)
                        {
                            ComponentesAromaticos.RemoveAt(index);
                            ComponentesAromaticos.Insert(index, componenteAromaticoEmEdicao);
                        }
                    }
                }

                if (sucesso)
                {
                    LimparCampos();
                    componenteAromaticoEmEdicao = null;
                }
                else
                {
                    await MostrarDialogoAviso("Falha ao salvar ou atualizar o insumo. Verifique os campos.");
                }
            }
            CarregarRegistrosAsync();
        }

        private void EditarInsumo_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var componentesAromaticos = button?.Tag as ComponenteAromaticoModel;

            if (componentesAromaticos != null)
            {
                componenteAromaticoEmEdicao = componentesAromaticos;
                NomeTextBox.Text = componentesAromaticos.Nome;
                NomeEnTextBox.Text = componentesAromaticos.NomenclaturaEn;
                CustoTextBox.Text = componentesAromaticos.Custo.ToString();
                CasTextBox.Text = componentesAromaticos.CAS;
                FemaTextBox.Text = componentesAromaticos.Fema;
                CodigoInternoTextBox.Text = componentesAromaticos.CodigoInterno;
                DescricaoTextBox.Text = componentesAromaticos.Descricao;
                LinkTextBox.Text = componentesAromaticos.LinkReferencia;
                CodigoFornecedorTextBox.Text = componentesAromaticos.CodigoProdutoFornecedor;
                FornecedorComboBox.SelectedItem = Fornecedores.FirstOrDefault(f => f.Id_Fornecedor == componentesAromaticos.IdFornecedor);
                SituacaoToggleSwitch.IsOn = componentesAromaticos.Situacao;
                // Converte DateTime para DateTimeOffset
                if (componentesAromaticos.DataRecebimento.HasValue == true)
                {
                    DataRecebimentoPicker.Date = new DateTimeOffset(componentesAromaticos.DataRecebimento.Value);
                }
                else
                {
                    DataRecebimentoPicker.Date = DateTimeOffset.Now;
                }
                if (componentesAromaticos.DataCadastro.HasValue == true)
                {
                    DataCadastroPicker.Date = new DateTimeOffset(componentesAromaticos.DataRecebimento.Value);
                }
                else
                {
                    DataCadastroPicker.Date = DateTimeOffset.Now;
                }
            }            
        }

        private void LimparCampos()
        {
            NomeTextBox.Text = string.Empty;
            CustoTextBox.Text = string.Empty;
            NomeEnTextBox.Text = string.Empty;
            CasTextBox.Text = string.Empty;
            FemaTextBox.Text = string.Empty;
            LinkTextBox.Text = string.Empty;
            FornecedorComboBox.SelectedIndex = -1;
            CodigoInternoTextBox.Text = string.Empty;
            DescricaoTextBox.Text = string.Empty;
            CodigoFornecedorTextBox.Text = string.Empty;
            componenteAromaticoEmEdicao = null;
            DataRecebimentoPicker.Date = DateTimeOffset.Now;
            DataCadastroPicker.Date = DateTimeOffset.Now;
        }
    }
}
