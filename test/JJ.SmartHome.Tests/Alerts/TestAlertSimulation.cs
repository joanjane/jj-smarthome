using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts
{
    public class TestAlertSimulation
    {
        [Fact]
        [Trait("TestCategory", "Integration")]
        [Trait("TestCategory", "DeviceOccupiedSimulation")]
        public async Task SimulateAqaraOccupancyDetectedEvent()
        {
            var payload = new AqaraOccupancySensorEvent
            {
                Occupancy = true,
                LinkQuality = 1,
                Battery = 100,
                Illuminance = 50,
                IlluminanceLux = 50,
                Voltage = 300,
            };
            await PublishTestMessage(payload, "MQTT:OccupancyTopic");
        }

        [Fact]
        [Trait("TestCategory", "Integration")]
        [Trait("TestCategory", "Zigbee2MqttPermitJoin")]
        public async Task SimulateZigbee2MqttPermitJoin()
        {
            await PublishTestMessage("true", "MQTT:Zigbee2MqttPermitJoinTopic");
        }

        private static async Task PublishTestMessage<T>(T payload, string topicSettingKey)
        {
            var configuration = Utils.ConfigBuilder.Build();

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

                if (payload is string)
                {
                    await mqttClient.Publish(topic, payload as string);
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
