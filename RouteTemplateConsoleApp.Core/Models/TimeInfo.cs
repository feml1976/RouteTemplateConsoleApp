using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public double Mileage { get; set; }

        /// <summary>
        /// Duración en segundos del segmento
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Tiempo de descanso en segundos (opcional)
        /// </summary>
        public int? Breaks { get; set; }
    }
}