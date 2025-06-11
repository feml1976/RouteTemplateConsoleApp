using Microsoft.Extensions.Logging;
using RouteTemplateConsoleApp.Application.Interfaces;
using RouteTemplateConsoleApp.Core.Interfaces;
using RouteTemplateConsoleApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para mostrar información de rutas en consola
    /// </summary>
    public class RouteDisplayService : IRouteDisplayService
    {
        private readonly IRouteTemplateService _routeTemplateService;
        private readonly ILogger<RouteDisplayService> _logger;

        /// <summary>
        /// Constructor del servicio de visualización
        /// </summary>
        /// <param name="routeTemplateService">Servicio de plantillas de rutas</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RouteDisplayService(
            IRouteTemplateService routeTemplateService,
            ILogger<RouteDisplayService> logger)
        {
            _routeTemplateService = routeTemplateService ?? throw new ArgumentNullException(nameof(routeTemplateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Muestra todas las plantillas de rutas en formato de consola
        /// </summary>
        /// <returns>Tarea asíncrona</returns>
        public async Task DisplayAllRouteTemplatesAsync()
        {
            try
            {
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    PLANTILLAS DE RUTAS                      ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                _logger.LogInformation("Iniciando visualización de plantillas de rutas");

                var routeTemplates = await _routeTemplateService.GetAllRouteTemplatesAsync();

                if (!routeTemplates.Any())
                {
                    Console.WriteLine("⚠️  No se encontraron plantillas de rutas.");
                    return;
                }

                for (int i = 0; i < routeTemplates.Count; i++)
                {
                    Console.WriteLine($"🚛 RUTA {i + 1} de {routeTemplates.Count}");
                    Console.WriteLine(new string('─', 80));
                    DisplayRouteTemplate(routeTemplates[i]);
                    Console.WriteLine();
                }

                DisplaySummary(routeTemplates);

                _logger.LogInformation("Visualización completada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al mostrar las plantillas de rutas");
                Console.WriteLine($"❌ Error al obtener las plantillas de rutas: {ex.Message}");
            }
        }

        /// <summary>
        /// Muestra una plantilla de ruta específica con formato detallado
        /// </summary>
        /// <param name="routeTemplate">Plantilla de ruta a mostrar</param>
        public void DisplayRouteTemplate(RouteTemplate routeTemplate)
        {
            if (routeTemplate == null)
            {
                Console.WriteLine("❌ Plantilla de ruta nula");
                return;
            }

            var totalDistance = _routeTemplateService.CalculateTotalDistance(routeTemplate);
            var formattedDuration = _routeTemplateService.FormatDuration(routeTemplate.Duration);

            Console.WriteLine($"📋 ID: {routeTemplate.Id}");
            Console.WriteLine($"🏷️  Nombre: {routeTemplate.Name}");
            Console.WriteLine($"🔗 Código: {routeTemplate.Code}");
            Console.WriteLine($"📍 Origen: {routeTemplate.DeparturePlace}");
            Console.WriteLine($"🎯 Destino: {routeTemplate.ArrivalPlace}");
            Console.WriteLine($"⏱️  Duración: {formattedDuration}");
            Console.WriteLine($"📏 Distancia Total: {totalDistance:N0} metros ({totalDistance / 1000:N1} km)");
            Console.WriteLine($"🔢 Número de Segmentos: {routeTemplate.NumberOfLegs}");
            Console.WriteLine($"👤 Usuario: {routeTemplate.Username}");
            Console.WriteLine($"📅 Fecha: {routeTemplate.Timestamp:yyyy-MM-dd HH:mm:ss}");

            if (routeTemplate.Steps?.Any() == true)
            {
                Console.WriteLine($"📋 SEGMENTOS DE LA RUTA:");
                for (int i = 0; i < routeTemplate.Steps.Count; i++)
                {
                    var step = routeTemplate.Steps[i];
                    var stepDuration = _routeTemplateService.FormatDuration(step.Times?.Duration ?? 0);
                    var stepDistance = step.Times?.Mileage ?? 0;

                    Console.WriteLine($"   {i + 1:D2}. {step.Starts?.Address} → {step.Arrive?.Address}");
                    Console.WriteLine($"       📏 {stepDistance:N0}m | ⏱️ {stepDuration}");

                    if (step.Times?.Breaks.HasValue == true && step.Times.Breaks > 0)
                    {
                        var breakTime = _routeTemplateService.FormatDuration(step.Times.Breaks.Value);
                        Console.WriteLine($"       ☕ Descanso: {breakTime}");
                    }
                }
            }
        }

        /// <summary>
        /// Muestra un resumen estadístico de todas las rutas
        /// </summary>
        /// <param name="routeTemplates">Lista de plantillas de rutas</param>
        public void DisplaySummary(List<RouteTemplate> routeTemplates)
        {
            if (!routeTemplates.Any())
                return;

            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                        RESUMEN                               ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            var totalRoutes = routeTemplates.Count;
            var totalDistance = routeTemplates.Sum(r => _routeTemplateService.CalculateTotalDistance(r));
            var totalDuration = routeTemplates.Sum(r => r.Duration);
            var averageLegs = routeTemplates.Average(r => r.NumberOfLegs);
            var routesByUser = routeTemplates.GroupBy(r => r.Username).ToDictionary(g => g.Key, g => g.Count());

            Console.WriteLine($"📊 Total de Rutas: {totalRoutes}");
            Console.WriteLine($"📏 Distancia Total: {totalDistance:N0} metros ({totalDistance / 1000:N1} km)");
            Console.WriteLine($"⏱️  Duración Total: {_routeTemplateService.FormatDuration(totalDuration)}");
            Console.WriteLine($"📊 Promedio de Segmentos: {averageLegs:F1}");
            Console.WriteLine();
            Console.WriteLine("👥 Rutas por Usuario:");
            foreach (var userRoute in routesByUser.OrderByDescending(ur => ur.Value))
            {
                Console.WriteLine($"   • {userRoute.Key}: {userRoute.Value} rutas");
            }
            Console.WriteLine();
        }
    }
}
