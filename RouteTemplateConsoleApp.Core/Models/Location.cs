using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// RouteTemplateConsoleApp.Core/Models/Location.cs
namespace RouteTemplateConsoleApp.Core.Models
{
    /// <summary>
    /// Representa una ubicación con identificador único, dirección y cliente asociado
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Identificador único del lugar
        /// </summary>
        public int PlaceId { get; set; }

        /// <summary>
        /// Dirección textual del lugar
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del cliente asociado
        /// </summary>
        public int ClientId { get; set; }
    }
}
