using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// RouteTemplateConsoleApp.Core/Models/RouteTemplate.cs
using System.Text.Json.Serialization;

namespace RouteTemplateConsoleApp.Core.Models
{
    /// <summary>
    /// Plantilla de ruta que contiene información completa de un recorrido logístico
    /// </summary>
    public class RouteTemplate
    {
        /// <summary>
        /// Identificador único de la plantilla de ruta
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre descriptivo de la ruta
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Número total de segmentos en la ruta
        /// </summary>
        public int NumberOfLegs { get; set; }

        /// <summary>
        /// Lugar de salida de la ruta
        /// </summary>
        public string DeparturePlace { get; set; } = string.Empty;

        /// <summary>
        /// Lugar de llegada de la ruta
        /// </summary>
        public string ArrivalPlace { get; set; } = string.Empty;

        /// <summary>
        /// Duración total estimada en segundos
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Código identificador de la ruta
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Color asociado a la ruta (valor numérico)
        /// </summary>
        public int Colour { get; set; }

        /// <summary>
        /// Marca de tiempo de creación/modificación
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Usuario que creó o modificó la plantilla
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Lista de pasos que componen la ruta
        /// </summary>
        public List<Step> Steps { get; set; } = new();

        /// <summary>
        /// Campo adicional personalizable 1
        /// </summary>
        public string Field1 { get; set; } = string.Empty;

        /// <summary>
        /// Campo adicional personalizable 2
        /// </summary>
        public string Field2 { get; set; } = string.Empty;

        /// <summary>
        /// Campo adicional personalizable 3
        /// </summary>
        public string Field3 { get; set; } = string.Empty;
    }
}