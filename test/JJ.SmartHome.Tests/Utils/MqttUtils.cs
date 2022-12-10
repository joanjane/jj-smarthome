using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Options;
using System.Text;

namespace JJ.SmartHome.Tests.Utils
{
    public class MqttUtils
    {
        public static async Task PublishTestMessage<T>(T payload, string topicSettingKey)
        {
            var configuration = ConfigBuilder.Build();

            var options = configuration.GetSection("MQTT")
                .Get<MqttClientOptions>();

            var topic = configuration[topicSettingKey];

            var logger = LoggerFactory.Create(c => c.AddConsole()).CreateLogger<MqttClient>();

            using (var mqttClient = new MqttClient(Options.Create(options), logger))
            {
                var waitToken = new CancellationTokenSource();
                await mqttClient.Connect("test", () =>
                {
                    waitToken.Cancel();
                    return Task.CompletedTask;
                });

                try
                {
                    await Task.Delay(8000, waitToken.Token); // Wait connection to be stablished
                }
                catch { }

                await mqttClient.Subscribe(topic, (message) =>
                {
                    var content = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
                    logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {content}");
                    return Task.CompletedTask;
                });

                if (payload is string stringPayload)
                {
                    await mqttClient.Publish(topic, stringPayload);
                }
                else
                {
                    await mqttClient.Publish(topic, payload);

                }
                await Task.Delay(5000);
                await mqttClient.Close();
            }
        }
    }
}
