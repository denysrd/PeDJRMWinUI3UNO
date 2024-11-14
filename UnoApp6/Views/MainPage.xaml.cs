// PeDJRMWinUI3UNO/MainPage.xaml.cs

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PeDJRMWinUI3UNO.Services;
using System;
using muxc = Microsoft.UI.Xaml.Controls;

namespace PeDJRMWinUI3UNO
{
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
        private void MainNavigationView_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
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
                var selectedItem = args.SelectedItem as muxc.NavigationViewItem;
                if (selectedItem != null)
                {
                    // Obtém a Tag do item selecionado para identificar a página de destino
                    string pageTag = selectedItem.Tag.ToString();

                    // Usa o NavigationService para navegar para a página correspondente
                    _navigationService.NavigateTo(pageTag);
                }
            }
        }

        // Evento acionado após a navegação no ContentFrame, usado para controlar o botão de voltar
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Define a visibilidade do botão de voltar com base na capacidade de voltar no histórico
            MainNavigationView.IsBackButtonVisible = _navigationService.CanGoBack
                ? muxc.NavigationViewBackButtonVisible.Visible
                : muxc.NavigationViewBackButtonVisible.Collapsed;
        }

        // Evento acionado quando o botão de voltar do NavigationView é pressionado
        private void MainNavigationView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            // Usa o NavigationService para voltar uma página no histórico, se possível
            _navigationService.NavigateBack();
        }
    }
}
