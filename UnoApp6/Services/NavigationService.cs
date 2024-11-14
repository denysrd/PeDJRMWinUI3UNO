using Microsoft.UI.Xaml.Controls;
using PeDJRMWinUI3UNO.Views;
using PeDJRMWinUI3UNO.Views.Cadastros;
using System;

namespace PeDJRMWinUI3UNO.Services
{
    public class NavigationService
    {
        private readonly Frame _contentFrame; // Frame usado para navegar entre páginas

        public NavigationService(Frame contentFrame)
        {
            _contentFrame = contentFrame;
        }

        public void NavigateTo(string pageTag)
        {
            // Define o tipo de página com base na Tag
            Type pageType = pageTag switch
            {
                "HomePage" => typeof(HomePage),   // Agora usa HomePage em vez de MainPage
                "CadastroPage" => typeof(CadastroPage),
                "ReceitaPage" => typeof(ReceitaPage),
                "FlavorizantesPage" => typeof(FlavorizantesView),
                _ => null
            };

            // Navega para a página se ela não for a página atual
            if (pageType != null && _contentFrame.CurrentSourcePageType != pageType)
            {
                _contentFrame.Navigate(pageType);
            }
        }

        public void NavigateBack()
        {
            if (_contentFrame.CanGoBack)
            {
                _contentFrame.GoBack();
            }
        }

        public bool CanGoBack => _contentFrame.CanGoBack;
    }
}
