using Microsoft.Extensions.Logging;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Infrastructure.Services
{
    /// <summary>
    /// Servicio de dominio para gestión de plantillas de rutas
    /// </summary>
    public class RouteTemplateService : IRouteTemplateService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<RouteTemplateService> _logger;

        /// <summary>
        /// Constructor del servicio de plantillas de rutas
        /// </summary>
        /// <param name="apiClient">Cliente de API para obtener datos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RouteTemplateService(IApiClient apiClient, ILogger<RouteTemplateService> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    }
}
