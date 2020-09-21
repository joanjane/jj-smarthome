using System;
using System.Text.Json.Serialization;

namespace JJ.SmartHome.Core.EnvSensors.Dto
{
    public class EnvSensorsEvent
    {
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }
        
        [JsonPropertyName("pressure")]
        public double? Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public double? Humidity { get; set; }

        public DateTime Time { get; set; }
    }
}
