using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// RouteTemplateConsoleApp.Core/Models/TimeInfo.cs
namespace RouteTemplateConsoleApp.Core.Models
{
    /// <summary>
    /// Información de tiempo y distancia para un segmento de ruta
    /// </summary>
    public class TimeInfo
    {
        /// <summary>
        /// Distancia en metros del segmento
        /// </summary>
        [JsonPropertyName("mileage")]
        public double Mileage { get; set; }

        /// <summary>
        /// Duración en segundos del segmento
        /// </summary>
        [JsonPropertyName("duration")] 
        public int Duration { get; set; }

        /// <summary>
        /// Tiempo de descanso en segundos (opcional)
        /// </summary>
        [JsonPropertyName("breaks")] 
        public int? Breaks { get; set; }
    }
}