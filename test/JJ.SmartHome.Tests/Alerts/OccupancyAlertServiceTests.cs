using System;
using System.Threading;
using System.Threading.Tasks;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Occupancy;
using JJ.SmartHome.Core.Occupancy.Dto.Sensors;
using JJ.SmartHome.Db.Entities;
using JJ.SmartHome.Tests.Builders;
using NSubstitute;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts
{
    public class OccupancyAlertServiceTests
    {
        [Fact]
        [Trait("TestCategory", "Unit")] // NOTE: Technically, this a is integration test
        public async Task GivenLastFiredAlertOutOfSnoozePeriod_WhenMotionOccupancyDetected_ThenNotifyAlert()
        {
            // Arrange
            var occupancyAlertServiceBuilder = new OccupancyAlertServiceBuilder()
                .WithListenerTopic(OccupancyAlertServiceBuilder.MotionOccupancyTopic)
                .WithLastFiredAlert(DateTimeOffset.Now.AddMinutes(-1))
                .WithSnoozeAlertPeriod(TimeSpan.FromSeconds(1))
                .WithDefaultDeviceConfiguration()
                .WithAlertNotifySucceeded()
                .WithWriteMeasureSucceeded();

            var sut = occupancyAlertServiceBuilder.Build();

            // Act
            var payload = new AqaraMotionOccupancySensorEvent()
            {
                Occupancy = true
            };
            await Act(
                sut,
                occupancyAlertServiceBuilder.MqttClient,
                OccupancyAlertServiceBuilder.MotionOccupancyTopic,
                payload);

            // Assert
            await occupancyAlertServiceBuilder.AlertNotifier.Received().Notify(Arg.Any<string>(), Arg.Any<string>());
            await occupancyAlertServiceBuilder.AlertsStore.Received().WriteMeasure(Arg.Is<AlertMeasure>((m) => m.Location == "motion"));
        }

        [Fact]
        [Trait("TestCategory", "Unit")]
        public async Task GivenLastFiredAlertInsideOfSnoozePeriod_WhenOccupancyDetected_ThenSkipAlertNotification()
        {
            // Arrange
            var occupancyAlertServiceBuilder = new OccupancyAlertServiceBuilder()
                .WithListenerTopic(OccupancyAlertServiceBuilder.MotionOccupancyTopic)
                .WithDefaultDeviceConfiguration()
                .WithLastFiredAlert(DateTimeOffset.Now.AddMinutes(-1))
                .WithSnoozeAlertPeriod(TimeSpan.FromMinutes(2));

            var sut = occupancyAlertServiceBuilder.Build();

            // Act
            var payload = new AqaraMotionOccupancySensorEvent()
            {
                Occupancy = true
            };
            await Act(
                sut,
                occupancyAlertServiceBuilder.MqttClient,
                OccupancyAlertServiceBuilder.MotionOccupancyTopic,
                payload);

            // Assert
            await occupancyAlertServiceBuilder.AlertNotifier.DidNotReceive().Notify(Arg.Any<string>(), Arg.Any<string>());
            await occupancyAlertServiceBuilder.AlertsStore.DidNotReceive().WriteMeasure(Arg.Any<AlertMeasure>());
        }

        [Fact]
        [Trait("TestCategory", "Unit")]
        public async Task GivenNonPreviousAlert_WhenMagnetOccupancyDetected_ThenNotifyAlert()
        {
            // Arrange
            var occupancyAlertServiceBuilder = new OccupancyAlertServiceBuilder()
                .WithListenerTopic(OccupancyAlertServiceBuilder.MagnetOccupancyTopic)
                .WithDefaultDeviceConfiguration()
                .WithoutPreviousAlert()
                .WithSnoozeAlertPeriod(TimeSpan.FromMinutes(1))
                .WithAlertNotifySucceeded()
                .WithWriteMeasureSucceeded();

            var sut = occupancyAlertServiceBuilder.Build();

            // Act
            var payload = new AqaraMagnetOccupancySensorEvent()
            {
                Contact = false
            };
            await Act(
                sut, 
                occupancyAlertServiceBuilder.MqttClient, 
                OccupancyAlertServiceBuilder.MagnetOccupancyTopic, 
                payload);

            // Assert
            await occupancyAlertServiceBuilder.AlertNotifier.Received().Notify(Arg.Any<string>(), Arg.Any<string>());
            await occupancyAlertServiceBuilder.AlertsStore.Received().WriteMeasure(Arg.Is<AlertMeasure>((m) => m.Location == "magnet"));
        }

        private static async Task Act<T>(OccupancyAlertBackgroundService sut, IMqttClient mqttClient, string topic, T payload)
        {
            var waitToken = new CancellationTokenSource();
            waitToken.CancelAfter(20000);

            await Task.WhenAll(
                mqttClient.Publish(
                    topic,
                    payload
                ),
                sut.StartAsync(waitToken.Token)
            );
        }
    }
}
