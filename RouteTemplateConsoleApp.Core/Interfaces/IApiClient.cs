using RouteTemplateConsoleApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Core.Interfaces
{
    /// <summary>
    /// Interfaz para cliente de API que obtiene plantillas de rutas
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Obtiene todas las plantillas de rutas desde la API de Frotcom
        /// </summary>
        /// <returns>Lista de plantillas de rutas</returns>
        /// <exception cref="HttpRequestException">Cuando la solicitud HTTP falla</exception>
        /// <exception cref="JsonException">Cuando la respuesta JSON no es válida</exception>
        Task<List<RouteTemplate>> GetRouteTemplatesAsync();
    }
}
