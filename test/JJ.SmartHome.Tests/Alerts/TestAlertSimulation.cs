using System;
using System.Threading.Tasks;
using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Tests.Mocks.Fixtures;
using JJ.SmartHome.Tests.Utils;
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
            var payload = new AqaraMotionOccupancySensorEvent
            {
                Occupancy = true,
                LinkQuality = 1,
                Battery = 100,
                Illuminance = 50,
                IlluminanceLux = 50,
                Voltage = 300,
            };
            await MqttUtils.PublishTestMessage(payload, "MQTT:MotionOccupancyTopic");
        }

        [Fact]
        [Trait("TestCategory", "Integration")]
        [Trait("TestCategory", "OcuppancyAlertSimulation")]
        public async Task SimulateOccupancyAlertEvent()
        {
            var payload = new OccupancyAlertEvent
            {
                Timestamp = DateTimeOffset.UtcNow,
                Fired = true,
                Location = "hall"
            };
            await MqttUtils.PublishTestMessage(payload, "MQTT:AlertOccupancyTopic");
        }


        [Trait("TestCategory", "Integration")]
        [Trait("TestCategory", "SensorSimulation")]
        [Theory]
        [InlineData(FixtureContants.MotionSensorPayload, "MQTT:MotionOccupancyTopic")]
        [InlineData(FixtureContants.MagnetSensorPayload, "MQTT:MagnetOccupancyTopic")]
        [InlineData(FixtureContants.WeatherSensorPayload, "MQTT:EnvSensorsSenseHatTopic")]
        public async Task SimulateSensorEvent(string fixture, string topic)
        {
            var payload = FixtureUtils.LoadFixture(fixture);
            await MqttUtils.PublishTestMessage(payload, topic);
        }

    }
}
