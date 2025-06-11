using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// RouteTemplateConsoleApp.Core/Models/Step.cs
namespace RouteTemplateConsoleApp.Core.Models
{
    /// <summary>
    /// Representa un paso individual dentro de una plantilla de ruta
    /// </summary>
    public class Step
    {
        /// <summary>
        /// Identificador único del paso
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ubicación de inicio del paso
        /// </summary>
        public Location Starts { get; set; } = new();

        /// <summary>
        /// Ubicación de llegada del paso
        /// </summary>
        public Location Arrive { get; set; } = new();

        /// <summary>
        /// Información de tiempo y distancia del paso
        /// </summary>
        public TimeInfo Times { get; set; } = new();
    }
}