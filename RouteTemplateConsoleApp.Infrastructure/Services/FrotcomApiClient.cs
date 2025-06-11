using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using RouteTemplateConsoleApp.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        /// <summary>
        /// Constructor del cliente de API
        /// </summary>
        /// <param name="httpClient">Cliente HTTP inyectado</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// <param name="apiConfig">Configuración de la API</param>
        public FrotcomApiClient(
            HttpClient httpClient,
            ILogger<FrotcomApiClient> logger,
            IOptions<ApiConfiguration> apiConfig)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiConfig = apiConfig?.Value ?? throw new ArgumentNullException(nameof(apiConfig));

            _httpClient.Timeout = TimeSpan.FromSeconds(_apiConfig.TimeoutSeconds);
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

                var url = $"{_apiConfig.BaseUrl}?api_key={_apiConfig.ApiKey}";
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
    }
}
