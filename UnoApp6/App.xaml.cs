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

        // Configuração de DbContext com MySQL
        try
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql("Server=localhost;Database=db_jrmsistema;User=root;Password=Neoo@@141189;",
                    new MySqlServerVersion(new Version(8, 0, 21))
                ));
        }
        catch (Exception ex)
        {
            // Exibe erro de conexão com o banco de dados (pode logar ou tratar conforme necessário)
            Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
        }

        // Adiciona repositórios e serviços
        services.AddTransient<FornecedorRepository>();
        services.AddTransient<FornecedorService>();
        services.AddTransient<TipoIngredienteRepository>();
        services.AddTransient<TipoIngredienteService>();
        services.AddTransient<TipoFormulacaoRepository>();
        services.AddTransient<TipoFormulacaoService>();
        services.AddTransient<InsumosService>();
        services.AddTransient<InsumosRepository>();
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
            new ViewMap<MainPage, MainViewModel>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new("Main", View: views.FindByViewModel<MainViewModel>(), IsDefault: true)
                ]
            )
        );
    }
}
