using PeDJRMWinUI3UNO.Views.Cadastros;

namespace PeDJRMWinUI3UNO
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainNavigationView.SelectedItem = MainNavigationView.MenuItems[0]; // Seleciona a página inicial
        }

        // Método chamado quando o item do NavigationView é alterado
        private async void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                // Verifica se o usuário pode sair da página atual
                bool canNavigate = await CheckUnsavedDataAsync();
                if (!canNavigate) return;

                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                // Verifica se o usuário pode sair da página atual
                bool canNavigate = await CheckUnsavedDataAsync();
                if (!canNavigate) return;

                // Obtém a tag do item selecionado e navega para a página correspondente
                string pageTag = selectedItem.Tag.ToString();
                Type pageType = GetPageTypeByTag(pageTag);

                if (pageType != null)
                {
                    ContentFrame.Navigate(pageType);
                }
            }
        }

        // Verifica se há dados não salvos antes de navegar
        private async Task<bool> CheckUnsavedDataAsync()
        {
            // Exemplo de verificação de dados não salvos
            bool hasUnsavedData = CheckForUnsavedData(); // Implemente sua lógica de verificação de dados não salvos

            if (hasUnsavedData)
            {
                var dialog = new ContentDialog
                {
                    Title = "Dados Não Salvos",
                    Content = "Você tem dados não salvos. Tem certeza de que deseja sair?",
                    PrimaryButtonText = "Sim",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();
                return result == ContentDialogResult.Primary;
            }

            return true;
        }

        // Simula a verificação de dados não salvos (implemente a lógica real conforme necessário)
        private bool CheckForUnsavedData()
        {
            // Retorne verdadeiro se houver dados não salvos, falso caso contrário
            return false; // Exemplo: retorne "true" para simular dados não salvos
        }

        // Retorna o tipo da página correspondente ao nome da tag
        private Type GetPageTypeByTag(string tag)
        {
            return tag switch
            {
                "HomePage" => typeof(HomePage),
                "CadastroPage" => typeof(CadastroPage),
                "ReceitaPage" => typeof(ReceitaPage),
                "SettingsPage" => typeof(SettingsPage),
                _ => null
            };
        }
    }
}
