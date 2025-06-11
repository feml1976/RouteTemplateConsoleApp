using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Infrastructure.Configuration
{
    /// <summary>
    /// Configuración para la API de Frotcom
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        /// URL base de la API
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Clave de API para autenticación
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Timeout para las peticiones HTTP en segundos
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
}
