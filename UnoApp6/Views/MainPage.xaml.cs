using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PeDJRMWinUI3UNO.Views.Cadastros;
using System;
using muxc = Microsoft.UI.Xaml.Controls;

namespace PeDJRMWinUI3UNO
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigated += ContentFrame_Navigated;
        }

        private void MainNavigationView_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = args.SelectedItem as muxc.NavigationViewItem;
                if (selectedItem != null)
                {
                    string pageTag = selectedItem.Tag.ToString();
                    Type pageType = pageTag switch
                    {
                        "HomePage" => typeof(HomePage),
                        "CadastroPage" => typeof(CadastroPage),
                        "ReceitaPage" => typeof(ReceitaPage),
                        _ => null
                    };

                    if (pageType == typeof(CadastroPage))
                    {
                        // Limpa o histórico de navegação para forçar a exibição da página inicial de CadastroPage
                        ContentFrame.BackStack.Clear();
                        ContentFrame.Navigate(typeof(CadastroPage));
                    }
                    else if (pageType != null && ContentFrame.CurrentSourcePageType != pageType)
                    {
                        ContentFrame.Navigate(pageType);
                    }
                }
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            MainNavigationView.IsBackButtonVisible = ContentFrame.CanGoBack
                ? muxc.NavigationViewBackButtonVisible.Visible
                : muxc.NavigationViewBackButtonVisible.Collapsed;
        }

        private void MainNavigationView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
    }
}
