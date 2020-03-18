using System.Text.Json.Serialization;

namespace JJ.SmartHome.Job.Dto
{
    public class AqaraOccupancySensorEvent
    {
        [JsonPropertyName("battery")]
        public int Battery { get; set; }
        [JsonPropertyName("voltage")]
        public int Voltage { get; set; }
        [JsonPropertyName("illuminance")]
        public int Illuminance { get; set; }
        [JsonPropertyName("illuminance_lux")]
        public int IlluminanceLux { get; set; }
        [JsonPropertyName("linkquality")]
        public int LinkQuality { get; set; }
        [JsonPropertyName("occupancy")]
        public bool Occupancy { get; set; }
    }
}
