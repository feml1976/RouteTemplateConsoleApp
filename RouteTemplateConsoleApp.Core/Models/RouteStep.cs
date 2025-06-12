using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Core.Models
{
    public class RouteStep
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("starts")]
        public Location Starts { get; set; } = new Location();

        [JsonPropertyName("arrive")]
        public Location Arrive { get; set; } = new Location();

        [JsonPropertyName("times")]
        public TimingInfo Times { get; set; } = new TimingInfo();
    }
}
