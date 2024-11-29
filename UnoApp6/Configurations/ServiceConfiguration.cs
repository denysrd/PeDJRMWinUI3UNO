// PeDJRMWinUI3UNO/Configurations/ServiceConfiguration.cs

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Repositories;
using PeDJRMWinUI3UNO.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace PeDJRMWinUI3UNO.Configurations
{
    public static class ServiceConfiguration
    {
        // Método responsável por registrar todos os serviços e repositórios da aplicação
        public static void RegisterServices(IServiceCollection services)
        {
            // Registro do ILoggerFactory para uso no DbContext
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information); // Nível mínimo de log
                builder.AddConsole(); // Adiciona saída para o console
            });

            // Configuração do DbContext com MySQL e logging detalhado
            try
            {
                services.AddDbContext<AppDbContext>((serviceProvider, options) =>
                {
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>(); // Obtém o ILoggerFactory
                    options.UseMySql("Server=localhost;Database=db_jrmsistema;User=root;Password=Neoo@@141189;",
                        new MySqlServerVersion(new Version(8, 0, 02)))
                        .UseLoggerFactory(loggerFactory) // Injeta o logger para capturar consultas SQL
                        .EnableSensitiveDataLogging() // Exibe valores dos parâmetros (cuidado com dados sensíveis)
                        .EnableDetailedErrors(); // Fornece detalhes adicionais sobre erros
                });
            }
            catch (Exception ex)
            {
                // Exibe erro de conexão com o banco de dados para debug ou logging
                Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
            }

            // Registro de repositórios para acesso a dados, permitindo injeção de dependência
            services.AddTransient<FornecedorRepository>();
            services.AddTransient<TipoIngredienteRepository>();
            services.AddTransient<TipoFormulacaoRepository>();
            services.AddTransient<InsumosRepository>();
            services.AddTransient<FlavorizantesRepository>();
            services.AddTransient<ReceitasRepository>();

            // Registro de serviços de negócios para serem usados nas Views e ViewModels
            services.AddTransient<FornecedorService>();
            services.AddTransient<TipoIngredienteService>();
            services.AddTransient<TipoFormulacaoService>();
            services.AddTransient<InsumosService>();
            services.AddTransient<FlavorizantesService>();
            services.AddTransient<ReceitasService>();

            services.AddTransient<IReceitasRepository, ReceitasRepository>();
            services.AddScoped<IVersoesReceitasRepository, VersoesReceitasRepository>(); // Registro do repositório
            services.AddScoped<VersoesReceitasService>(); // Registro do serviço
            services.AddScoped<IReceitasInsumosRepository, ReceitasInsumosRepository>(); // Registro do repositório
            services.AddScoped<ReceitasInsumosService>(); // Registro do serviço
        }
    }
}
