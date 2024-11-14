using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Repositories;
using PeDJRMWinUI3UNO.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Uno.Extensions.Configuration;
using PeDJRMWinUI3UNO.ViewModels;
using PeDJRMWinUI3UNO.Views.Cadastros;
using PeDJRMWinUI3UNO.Configurations;
namespace PeDJRMWinUI3UNO;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App()
    {
        this.InitializeComponent();
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Registra os serviços e repositórios usando ServiceConfiguration
        ServiceConfiguration.RegisterServices(services);

        Services = services.BuildServiceProvider();
    }

    protected Window? MainWindow { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    logBuilder
                        .SetMinimumLevel(context.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning)
                        .CoreLogLevel(LogLevel.Warning);
                }, enableUnoLogging: true)
                .UseConfiguration()
                .UseLocalization()
                .ConfigureServices((context, services) =>
                {
                    // Registro de serviços adicionais
                })
                .UseNavigation(RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.EnableHotReload();
#endif

        await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<FlavorizantesView, FlavorizantesViewModel>() // Adiciona FlavorizantesView
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new("Main", View: views.FindByViewModel<MainViewModel>(), IsDefault: true),
                    new("Flavorizantes", View: views.FindByViewModel<FlavorizantesViewModel>()) // Rota para FlavorizantesView
                ]
            )
        );
    }
}
