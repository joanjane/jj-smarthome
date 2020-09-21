using System.Text.Json.Serialization;

namespace JJ.SmartHome.Core.Alerts.Dto
{
    public class AlarmStatusEvent
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
