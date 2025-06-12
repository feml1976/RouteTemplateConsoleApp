using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonPropertyName("placeId")] 
        public int PlaceId { get; set; }

        /// <summary>
        /// Dirección textual del lugar
        /// </summary>
        [JsonPropertyName("address")] 
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del cliente asociado
        /// </summary>
        [JsonPropertyName("clientId")] 
        public int ClientId { get; set; }
    }
}
