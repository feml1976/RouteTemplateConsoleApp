using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using RouteTemplateConsoleApp.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Infrastructure.Services
{
    /// <summary>
    /// Cliente para la API de Frotcom que obtiene plantillas de rutas
    /// </summary>
    public class FrotcomApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FrotcomApiClient> _logger;
        private readonly ApiConfiguration _apiConfig;
        private readonly AuthenticationSettings _apiKey;

        private readonly IRouteRepository _routeRepository;

        /// <summary>
        /// Constructor del cliente de API
        /// </summary>
        /// <param name="httpClient">Cliente HTTP inyectado</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// <param name="apiConfig">Configuración de la API</param>
        public FrotcomApiClient(
            HttpClient httpClient,
            ILogger<FrotcomApiClient> logger,
            IOptions<ApiConfiguration> apiConfig,
            IOptions<AuthenticationSettings> apiKey,
            IRouteRepository routeRepository)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiConfig = apiConfig?.Value ?? throw new ArgumentNullException(nameof(apiConfig));

            _httpClient.Timeout = TimeSpan.FromSeconds(_apiConfig.TimeoutSeconds);
            _apiKey = apiKey?.Value ?? throw new ArgumentNullException(nameof(apiConfig));
            _routeRepository = routeRepository;
        }

        /// <summary>
        /// Obtiene todas las plantillas de rutas desde la API de Frotcom
        /// </summary>
        /// <returns>Lista de plantillas de rutas</returns>
        /// <exception cref="HttpRequestException">Cuando la solicitud HTTP falla</exception>
        /// <exception cref="JsonException">Cuando la respuesta JSON no es válida</exception>
        public async Task<List<RouteTemplate>> GetRouteTemplatesAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando solicitud a la API de Frotcom");

                var token = await GetToken(_apiKey);
                //var url = $"{_apiConfig.BaseUrl}?api_key={_apiConfig.ApiKey}";
                var url = $"{_apiConfig.BaseUrl}?api_key={token.token}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error en la respuesta de la API: {StatusCode} - {ReasonPhrase}",
                        response.StatusCode, response.ReasonPhrase);
                    throw new HttpRequestException($"Error al obtener datos de la API: {response.StatusCode}");
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Respuesta JSON recibida: {JsonLength} caracteres", jsonContent.Length);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var routeTemplates = JsonSerializer.Deserialize<List<RouteTemplate>>(jsonContent, options);

                if (routeTemplates == null)
                {
                    _logger.LogWarning("La deserialización retornó null");
                    return new List<RouteTemplate>();
                }

                var addRecords = await AddRecords(jsonContent);

                _logger.LogInformation("Se obtuvieron {Count} plantillas de rutas exitosamente", routeTemplates.Count);
                return routeTemplates;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de HTTP al obtener plantillas de rutas");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar la respuesta JSON");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener plantillas de rutas");
                throw;
            }
        }

        private async Task<string> AddRecords(string jsonContent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Respuesta de API recibida. Tamaño: {Size} caracteres", jsonContent.Length);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var routes = JsonSerializer.Deserialize<List<RouteTemplate>>(jsonContent, options);

            if (routes == null || !routes.Any())
            {
                _logger.LogWarning("No se encontraron rutas en la respuesta de la API");
                return "No se encontraron rutas en la respuesta de la API";
            }

            _logger.LogInformation("Deserialización exitosa. {Count} rutas encontradas", routes.Count);

            var routesToInsert = new List<GetRouteWithStep>();

            foreach (var template in routes)
            {
                var routeWithStep = MapToGetRouteWithStep(template);
                routesToInsert.Add(routeWithStep);
            }

            _logger.LogInformation("Mapeando {Count} rutas para inserción", routesToInsert.Count);

            var insertResult = await _routeRepository.InsertRoutesAsync(routesToInsert, cancellationToken);

            if (insertResult)
            {
                _logger.LogInformation("Procesamiento completado exitosamente. {Count} rutas insertadas", routesToInsert.Count);
            }
            else
            {
                _logger.LogError("Error al insertar las rutas en la base de datos");
            }

            return "Registros Almacenados";
        }

        private GetRouteWithStep MapToGetRouteWithStep(RouteTemplate template)
        {
            // Calcular Metros (sumatoria de mileage)
            var totalMetros = template.Steps?.Sum(s => s.Times?.Mileage ?? 0) ?? 0;

            // Calcular Segundos (sumatoria de duration)
            var totalSegundos = template.Steps?.Sum(s => s.Times?.Duration ?? 0) ?? 0;

            // Serializar Steps como JSON
            var stepsJson = template.Steps != null && template.Steps.Any()
                ? JsonSerializer.Serialize(template.Steps, new JsonSerializerOptions { WriteIndented = false })
                : "[]";

            var now = DateTime.UtcNow;

            return new GetRouteWithStep
            {
                RouteId = template.Id,
                Name = template.Name,
                NumberOfLegs = template.NumberOfLegs,
                DeparturePlace = template.DeparturePlace,
                ArrivalPlace = template.ArrivalPlace,
                Code = template.Code,
                TimeStamp = template.Timestamp,
                UserName = template.Username,
                Metros = totalMetros,
                Segundos = totalSegundos,
                Steps = stepsJson,
                State = 1,
                CreateAt = now,
                UpdateAt = now
            };
        }

        private async Task<FrotcomAuthentication> GetToken(AuthenticationSettings apiKey)
        {
            // Serializar el objeto a JSON  
            var json = JsonSerializer.Serialize(apiKey);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                // Hacer la petición POST  
                var response = await _httpClient.PostAsync(apiKey.BaseUrl, content);

                // Verificar si la respuesta es exitosa  
                response.EnsureSuccessStatusCode();

                // Leer la respuesta  
                var responseJson = await response.Content.ReadAsStringAsync();

                // Deserializar a la clase  
                var token = JsonSerializer.Deserialize<FrotcomAuthentication>(responseJson);

                return token ?? new FrotcomAuthentication();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la petición: {ex.Message}");
                return new FrotcomAuthentication();
            }
        }
    }
}
