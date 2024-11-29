// PeDJRMWinUI3UNO/MainPage.xaml.cs

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PeDJRMWinUI3UNO.Services;
using System;
using System.Diagnostics;
using MuxControls = Microsoft.UI.Xaml.Controls;


namespace PeDJRMWinUI3UNO;

public sealed partial class MainPage : Page
{
    // Instância do NavigationService para gerenciar a navegação de conteúdo dentro do ContentFrame
    private readonly NavigationService _navigationService;

    // Construtor da MainPage
    public MainPage()
    {
        this.InitializeComponent(); // Inicializa os componentes visuais da página

        // Inicializa o NavigationService passando o ContentFrame onde o conteúdo será carregado
        _navigationService = new NavigationService(ContentFrame);

        // Associa o evento Navigated do ContentFrame para controlar o botão de voltar
        ContentFrame.Navigated += ContentFrame_Navigated;

        // Define a HomePage como a página inicial ao carregar a MainPage
        _navigationService.NavigateTo("HomePage");
    }

    // Evento acionado quando um item do NavigationView é selecionado
    private void MainNavigationView_SelectionChanged(MuxControls.NavigationView sender, MuxControls.NavigationViewSelectionChangedEventArgs args)
    {
        // Verifica se o item selecionado é a opção de Configurações
        if (args.IsSettingsSelected)
        {
            // Navega diretamente para a página de configurações
            ContentFrame.Navigate(typeof(SettingsPage));
        }
        else
        {
            // Obtém o item selecionado no NavigationView
            var selectedItem = args.SelectedItem as MuxControls.NavigationViewItem;

            // Verifica se o item selecionado e sua Tag não são nulos
            if (selectedItem != null && selectedItem.Tag != null)
            {
                // Obtém a Tag do item selecionado para identificar a página de destino
                string? pageTag = selectedItem.Tag.ToString();

                // Verifica se o parâmetro 'pageTag' não é nulo ou vazio antes de navegar
                if (!string.IsNullOrEmpty(pageTag))
                {
                    // O parâmetro 'pageTag' é válido, prossegue com a navegação.
                    _navigationService.NavigateTo(pageTag);
                }
                else
                {
                    // Loga ou trata o caso onde 'pageTag' é nulo ou inválido.
                    Debug.WriteLine("Erro: O parâmetro 'pageTag' é nulo ou vazio. Navegação abortada.");
                }
            }
            else
            {
                // Loga ou trata o caso onde o item selecionado ou sua Tag são nulos.
                Debug.WriteLine("Erro: O item selecionado ou sua Tag são nulos.");
            }
        }
    }

    // Evento acionado após a navegação no ContentFrame.
    // Este método é usado para ajustar a visibilidade do botão de voltar no NavigationView
    // com base na possibilidade de navegar para trás no histórico de navegação.
    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        // Verifica se a API está sendo executada em um sistema operacional Windows.
        // A API utilizada para ajustar a visibilidade do botão de voltar pode não ser suportada em outras plataformas.
        if (OperatingSystem.IsWindows())
        {
            // Define a visibilidade do botão de voltar.
            // Se o histórico permitir navegação para trás, o botão será configurado como visível.
            // Caso contrário, o botão será configurado como oculto.
            MainNavigationView.IsBackButtonVisible = _navigationService.CanGoBack
                ? MuxControls.NavigationViewBackButtonVisible.Visible // Botão de voltar visível.
                : MuxControls.NavigationViewBackButtonVisible.Collapsed; // Botão de voltar oculto.
        }
        else
        {
            // Se a plataforma não for Windows, a funcionalidade do botão de voltar é ignorada.
            // Aqui pode-se adicionar um log para rastrear casos em que a API é chamada em plataformas não compatíveis.
            Debug.WriteLine("Aviso: A plataforma atual não suporta a API de visibilidade do botão de voltar.");
        }
    }

    // Evento acionado quando o botão de voltar do NavigationView é pressionado
    private void MainNavigationView_BackRequested(MuxControls.NavigationView sender, MuxControls.NavigationViewBackRequestedEventArgs args)
    {
        // Usa o NavigationService para voltar uma página no histórico, se possível
        _navigationService.NavigateBack();
    }
}
