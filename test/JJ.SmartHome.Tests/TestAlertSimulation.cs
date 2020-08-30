using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests
{
    public class TestAlertSimulation
    {
        [Fact]
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
            await PublishTestMessage(payload);
        }

        private static async Task PublishTestMessage(AqaraOccupancySensorEvent payload)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .Build();
            
            var options = configuration.GetSection("MQTT")
                .Get<MqttClientOptions>();

            var topic = configuration["MQTT:Topic"];

            var logger = LoggerFactory.Create(c => c.AddConsole()).CreateLogger<MqttClient>();

            using (var mqttClient = new MqttClient(Options.Create(options), logger))
            {
                var waitToken = new CancellationTokenSource();
                await mqttClient.Connect(() =>
                {
                    waitToken.Cancel();
                });
                
                try
                {
                    await Task.Delay(8000, waitToken.Token); // Wait connection to be stablished
                }
                catch { }
                
                mqttClient.Subscribe(topic, async (message) => {
                    var content = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
                    logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {content}");
                });

                await mqttClient.Publish(topic, payload);
                await Task.Delay(5000);
                await mqttClient.Close();
            }
        }
    }
}
