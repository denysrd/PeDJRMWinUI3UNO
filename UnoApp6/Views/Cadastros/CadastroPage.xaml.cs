namespace PeDJRMWinUI3UNO.Views.Cadastros;

public sealed partial class CadastroPage : Page
{
    public CadastroPage()
    {
        this.InitializeComponent();
    }
    
    private void NavigateToFlavorizantes(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(FlavorizantesView));
    }

    private void NavigateToInsumos(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(InsumosView));
    }

    private void NavigateToFornecedores(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(Fornecedor.FornecedorPage));
    }

    private void NavigateToCadastrosAuxiliares(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CadastrosAuxiliares.CadastrosAuxiliaresView));
    }
}
