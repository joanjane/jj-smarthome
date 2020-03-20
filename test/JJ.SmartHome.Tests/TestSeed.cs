using JJ.SmartHome.Core;
using JJ.SmartHome.Job.Dto;
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
        public async Task TestOccupancyDetectedEvent()
        {
            var payload = new AqaraOccupancySensorEvent
            {
                LinkQuality = 1,
                Battery = 100,
                Illuminance = 50,
                IlluminanceLux = 50,
                Voltage = 300,
                Occupancy = true
            };
            await PublishTestMessage(payload);
        }

        private static async Task PublishTestMessage(AqaraOccupancySensorEvent payload)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .Build();
            var options = configuration
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