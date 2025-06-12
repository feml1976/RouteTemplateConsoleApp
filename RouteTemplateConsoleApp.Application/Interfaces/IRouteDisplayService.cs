using RouteTemplateConsoleApp.Core.Models;

namespace RouteTemplateConsoleApp.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para mostrar información de rutas en consola
    /// </summary>
    public interface IRouteDisplayService
    {
        /// <summary>
        /// Muestra todas las plantillas de rutas en formato de consola
        /// </summary>
        /// <returns>Tarea asíncrona</returns>
        Task DisplayAllRouteTemplatesAsync();

        /// <summary>
        /// Muestra una plantilla de ruta específica con formato detallado
        /// </summary>
        /// <param name="routeTemplate">Plantilla de ruta a mostrar</param>
        void DisplayRouteTemplate(RouteTemplate routeTemplate);

        /// <summary>
        /// Muestra un resumen estadístico de todas las rutas
        /// </summary>
        /// <param name="routeTemplates">Lista de plantillas de rutas</param>
        void DisplaySummary(List<RouteTemplate> routeTemplates);
    }
}
