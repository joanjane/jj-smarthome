using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests
{
    public class TestSeed
    {
        [Fact]
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
            var options = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .Build()
                .GetSection("MQTT")
                .Get<MqttClientOptions>();

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
                await mqttClient.Publish(options.Topic, payload);
                await Task.Delay(5000);
                await mqttClient.Close();
            }
        }
    }
}
