using System.Text.Json.Serialization;

namespace JJ.SmartHome.Core.Alerts.Dto
{
    public record AqaraOccupancySensorEvent
    {
        public int Battery { get; set; }
        
        public int Voltage { get; set; }
        
        public int Illuminance { get; set; }

        [JsonPropertyName("illuminance_lux")]
        public int IlluminanceLux { get; set; }

        [JsonPropertyName("linkquality")]
        
        public int LinkQuality { get; set; }
        
        public bool Occupancy { get; set; }
    }
}
