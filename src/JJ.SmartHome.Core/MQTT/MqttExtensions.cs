using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace JJ.SmartHome.Core.Extensions
{
    public static class MqttExtensions
    {
        public static T DeserializeMqttMessage<T>(this MqttApplicationMessageReceivedEventArgs message, ILogger logger = null)
        {
            var payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);

            logger?.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {payload}");

            return JsonSerializer.Deserialize<T>(payload);
        }
    }
}
