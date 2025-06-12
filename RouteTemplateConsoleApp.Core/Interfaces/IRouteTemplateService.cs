using RouteTemplateConsoleApp.Core.Models;

namespace RouteTemplateConsoleApp.Core.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de plantillas de rutas
    /// </summary>
    public interface IRouteTemplateService
    {
        /// <summary>
        /// Obtiene y procesa todas las plantillas de rutas disponibles
        /// </summary>
        /// <returns>Lista de plantillas de rutas procesadas</returns>
        Task<List<RouteTemplate>> GetAllRouteTemplatesAsync();

        /// <summary>
        /// Calcula la distancia total de una plantilla de ruta
        /// </summary>
        /// <param name="routeTemplate">Plantilla de ruta a procesar</param>
        /// <returns>Distancia total en metros</returns>
        double CalculateTotalDistance(RouteTemplate routeTemplate);

        /// <summary>
        /// Calcula el tiempo total de una plantilla de ruta
        /// </summary>
        /// <param name="routeTemplate">Plantilla de ruta a procesar</param>
        /// <returns>Distancia total en metros</returns>
        double CalculateTotalDuration(RouteTemplate routeTemplate);
        

        /// <summary>
        /// Formatea la duración en un formato legible
        /// </summary>
        /// <param name="durationInSeconds">Duración en segundos</param>
        /// <returns>Duración formateada como string</returns>
        string FormatDuration(int durationInSeconds);

        /// <summary>
        /// Formatea la duración en un formato legible
        /// </summary>
        /// <param name="getRouteWithStep">Duración en segundos</param>
        /// <returns>Duración formateada como string</returns>
        string AddRecords(GetRouteWithStep getRouteWithStep);

        Task<List<RouteTemplate>> GetRouteTemplatesAsync(CancellationToken cancellationToken = default);
    }
}
