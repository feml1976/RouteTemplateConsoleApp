using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Core.Models
{
    public class TimingInfo
    {
        [JsonPropertyName("mileage")]
        public double Mileage { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("breaks")]
        public int? Breaks { get; set; }
    }
}
