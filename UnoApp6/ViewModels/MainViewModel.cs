using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using CommunityToolkit.Mvvm.Input;
namespace PeDJRMWinUI3UNO.ViewModels;

public partial class MainViewModel
{
    private INavigator _navigator;


    private string? name;

    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
        // GoToSecond = new AsyncRelayCommand(GoToSecondView);
    }
    public string? Title { get; }

    public ICommand GoToSecond { get; }

    // private async Task GoToSecondView()
    // {
    //     await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Name!));
    // }

}
