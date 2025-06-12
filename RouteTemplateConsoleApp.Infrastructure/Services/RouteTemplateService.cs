using Microsoft.Extensions.Logging;
using RouteTemplateConsoleApp.Core.Exceptions;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using System.Text.Json;

namespace RouteTemplateConsoleApp.Infrastructure.Services
{
    /// <summary>
    /// Servicio de dominio para gestión de plantillas de rutas
    /// </summary>
    public class RouteTemplateService : IRouteTemplateService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<RouteTemplateService> _logger;
        //private readonly IRouteProcessingService _routeProcessingService;

        private readonly HttpClient _httpClient;
        private const string API_URL = "https://v2api.frotcom.com/v2/routes/templatesWithSteps?api_key=da9c8b2c-f9e6-445a-9b9f-c31b1250ce4a";

        /// <summary>
        /// Constructor del servicio de plantillas de rutas
        /// </summary>
        /// <param name="apiClient">Cliente de API para obtener datos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RouteTemplateService(IApiClient apiClient, ILogger<RouteTemplateService> logger)//, IRouteProcessingService routeProcessingService)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_routeProcessingService = routeProcessingService;
        }

        /// <summary>
        /// Obtiene y procesa todas las plantillas de rutas disponibles
        /// </summary>
        /// <returns>Lista de plantillas de rutas procesadas</returns>
        public async Task<List<RouteTemplate>> GetAllRouteTemplatesAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las plantillas de rutas");
                var routeTemplates = await _apiClient.GetRouteTemplatesAsync();

                _logger.LogInformation("Procesando {Count} plantillas de rutas", routeTemplates.Count);

                // Aquí se podría agregar lógica adicional de procesamiento si es necesario
                // Por ejemplo: validaciones, transformaciones, etc.

                return routeTemplates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener plantillas de rutas");
                throw;
            }
        }

        /// <summary>
        /// Calcula la distancia total de una plantilla de ruta
        /// </summary>
        /// <param name="routeTemplate">Plantilla de ruta a procesar</param>
        /// <returns>Distancia total en metros</returns>
        public double CalculateTotalDistance(RouteTemplate routeTemplate)
        {
            if (routeTemplate?.Steps == null)
                return 0;

            return routeTemplate.Steps.Sum(step => step.Times?.Mileage ?? 0);
        }

        /// <summary>
        /// Calcula la distancia total de una plantilla de ruta
        /// </summary>
        /// <param name="routeTemplate">Plantilla de ruta a procesar</param>
        /// <returns>Distancia total en metros</returns>
        public double CalculateTotalDuration(RouteTemplate routeTemplate)
        {
            if (routeTemplate?.Steps == null)
                return 0;

            return routeTemplate.Steps.Sum(step => step.Times?.Duration ?? 0);
        }

        /// <summary>
        /// Formatea la duración en un formato legible
        /// </summary>
        /// <param name="durationInSeconds">Duración en segundos</param>
        /// <returns>Duración formateada como string</returns>
        public string FormatDuration(int durationInSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(durationInSeconds);

            if (timeSpan.TotalDays >= 1)
            {
                return $"{(int)timeSpan.TotalDays}d {timeSpan.Hours}h {timeSpan.Minutes}m";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
            }
            else
            {
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            }
        }

        /// <summary>
        /// Formatea la duración en un formato legible
        /// </summary>
        /// <param name="durationInSeconds">Duración en segundos</param>
        /// <returns>Duración formateada como string</returns>
        public async Task<string> AddRecords(GetRouteWithStep getRouteWithStep)
        {
            // Procesar rutas            
            //await _routeProcessingService.ProcessRoutesAsync();
            return string.Empty;
        }

        public async Task<List<RouteTemplate>> GetRouteTemplatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Iniciando consumo de API: {ApiUrl}", API_URL);

                var response = await _httpClient.GetAsync(API_URL, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new ApiException($"Error al consumir API. StatusCode: {response.StatusCode}, Content: {errorContent}");
                }

                var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("Respuesta de API recibida. Tamaño: {Size} caracteres", jsonContent.Length);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var routes = JsonSerializer.Deserialize<List<RouteTemplate>>(jsonContent, options);

                if (routes == null || !routes.Any())
                {
                    _logger.LogWarning("No se encontraron rutas en la respuesta de la API");
                    return new List<RouteTemplate>();
                }

                _logger.LogInformation("Deserialización exitosa. {Count} rutas encontradas", routes.Count);
                return routes;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError(ex, "Timeout al consumir la API");
                throw new ApiException("Timeout al consumir la API", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al consumir la API");
                throw new ApiException("Error de conexión al consumir la API", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar la respuesta JSON");
                throw new ApiException("Error al deserializar la respuesta JSON", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consumir la API");
                throw new ApiException("Error inesperado al consumir la API", ex);
            }
        }

        string IRouteTemplateService.AddRecords(GetRouteWithStep getRouteWithStep)
        {
            //_routeProcessingService.ProcessRoutesAsync();
            return string.Empty;
        }
    }
}
