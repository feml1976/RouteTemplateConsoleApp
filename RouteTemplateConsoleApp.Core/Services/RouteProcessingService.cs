using Microsoft.Extensions.Logging;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using System.Text.Json;

namespace RouteTemplateConsoleApp.Core.Services
{
    public class RouteProcessingService : IRouteProcessingService
    {
        private readonly IRouteTemplateService _routeTemplateService;
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<RouteProcessingService> _logger;

        public RouteProcessingService(
            IRouteTemplateService routeTemplateService,
            IRouteRepository routeRepository,
            ILogger<RouteProcessingService> logger)
        {
            _routeTemplateService = routeTemplateService;
            _routeRepository = routeRepository;
            _logger = logger;
        }

        public async Task ProcessRoutesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Iniciando procesamiento de rutas");

                // Asegurar que la tabla existe
                await _routeRepository.CreateTableIfNotExistsAsync(cancellationToken);

                var routeTemplates = await _routeTemplateService.GetRouteTemplatesAsync(cancellationToken);

                if (!routeTemplates.Any())
                {
                    _logger.LogWarning("No se encontraron rutas para procesar");
                    return;
                }

                var routesToInsert = new List<GetRouteWithStep>();

                foreach (var template in routeTemplates)
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el procesamiento de rutas");
                throw;
            }
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
    }
}
