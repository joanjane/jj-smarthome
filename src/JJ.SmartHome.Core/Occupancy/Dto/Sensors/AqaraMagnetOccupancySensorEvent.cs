using System.Text.Json.Serialization;

namespace JJ.SmartHome.Core.Occupancy.Dto.Sensors
{
    public record AqaraMagnetOccupancySensorEvent
    {
        public bool Contact { get; set; }

        public int Battery { get; set; }

        public int Voltage { get; set; }

        [JsonPropertyName("linkquality")]
        public int LinkQuality { get; set; }

        [JsonPropertyName("device_temperature")]
        public int DeviceTemperature { get; set; }

        [JsonPropertyName("power_outage_count")]
        public int PowerOutageCount { get; set; }
    }
}
