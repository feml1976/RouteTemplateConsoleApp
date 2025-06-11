using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RouteTemplateConsoleApp.Application.Interfaces;
using RouteTemplateConsoleApp.Application.Services;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Infrastructure.Configuration;
using RouteTemplateConsoleApp.Infrastructure.Services;

/// <summary>
/// Aplicación de consola para obtener y mostrar plantillas de rutas desde la API de Frotcom
/// </summary>
namespace RouteTemplateConsoleApp
{
    /// <summary>
    /// Clase principal de la aplicación
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos</param>
        /// <returns>Código de salida de la aplicación</returns>
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Crear el host de la aplicación con inyección de dependencias
                var host = CreateHostBuilder(args).Build();

                // Configurar la codificación de la consola para caracteres especiales
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                Console.WriteLine("🚀 Iniciando aplicación de Plantillas de Rutas...");
                Console.WriteLine();

                // Obtener el servicio de visualización y ejecutar la aplicación
                var displayService = host.Services.GetRequiredService<IRouteDisplayService>();
                await displayService.DisplayAllRouteTemplatesAsync();

                Console.WriteLine("✅ Aplicación finalizada exitosamente.");
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error crítico en la aplicación: {ex.Message}");
                Console.WriteLine($"📄 Detalles: {ex}");
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();
                return 1;
            }
        }

        /// <summary>
        /// Configura el host de la aplicación con servicios e inyección de dependencias
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos</param>
        /// <returns>Constructor del host configurado</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    // Configuración
                    services.Configure<ApiConfiguration>(
                        context.Configuration.GetSection("FrotcomApi"));

                    services.Configure<AuthenticationSettings>(
                        context.Configuration.GetSection("AuthenticationSettings"));

                    // HttpClient
                    services.AddHttpClient<IApiClient, FrotcomApiClient>();

                    // Servicios de infraestructura
                    services.AddScoped<IApiClient, FrotcomApiClient>();
                    services.AddScoped<IRouteTemplateService, RouteTemplateService>();

                    // Servicios de aplicación
                    services.AddScoped<IRouteDisplayService, RouteDisplayService>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Information);
                });
    }
}