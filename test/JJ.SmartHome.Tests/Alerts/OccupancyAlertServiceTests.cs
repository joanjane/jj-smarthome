using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Db.Entities;
using JJ.SmartHome.Tests.Builders;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts
{
    public class OccupancyAlertServiceTests
    {
        [Fact]
        [Trait("TestCategory", "Unit")]
        public async Task GivenLastFiredAlertOutOfSnoozePeriod_WhenOccupancyDetected_ThenNotifyAlert()
        {
            // Arrange
            var occupancyAlertServiceBuilder = new OccupancyAlertServiceBuilder()
                .WithLastFiredAlert(DateTimeOffset.Now.AddMinutes(-1))
                .WithSnoozeAlertPeriod(TimeSpan.FromSeconds(1))
                .WithAlertNotifySucceeded()
                .WithWriteMeasureSucceeded();

            var sut = occupancyAlertServiceBuilder.Build();

            var waitToken = new CancellationTokenSource();
            waitToken.CancelAfter(2000);

            // Act
            await Task.WhenAll(
                occupancyAlertServiceBuilder.MqttClient.Publish(
                    OccupancyAlertServiceBuilder.OccupancyTopic, 
                    new AqaraOccupancySensorEvent()
                    {
                        Occupancy = true
                    }
                ),
                sut.StartAsync(waitToken.Token)
            );

            // Assert
            await occupancyAlertServiceBuilder.AlertNotifier.Received().Notify(Arg.Any<string>(), Arg.Any<string>());
            await occupancyAlertServiceBuilder.AlertsStore.Received().WriteMeasure(Arg.Is<AlertMeasure>((m) => m.Location == "test"));
        }

        [Fact]
        [Trait("TestCategory", "Unit")]
        public async Task GivenLastFiredAlertInsideOfSnoozePeriod_WhenOccupancyDetected_ThenSkipAlertNotification()
        {
            // Arrange
            var occupancyAlertServiceBuilder = new OccupancyAlertServiceBuilder()
                .WithLastFiredAlert(DateTimeOffset.Now.AddMinutes(-1))
                .WithSnoozeAlertPeriod(TimeSpan.FromMinutes(5));

            var sut = occupancyAlertServiceBuilder.Build();

            var waitToken = new CancellationTokenSource();
            waitToken.CancelAfter(2000);

            // Act
            await Task.WhenAll(
                occupancyAlertServiceBuilder.MqttClient.Publish(
                    OccupancyAlertServiceBuilder.OccupancyTopic, 
                    new AqaraOccupancySensorEvent()
                    {
                        Occupancy = true
                    }
                ),
                sut.StartAsync(waitToken.Token)
            );

            // Assert
            await occupancyAlertServiceBuilder.AlertNotifier.DidNotReceive().Notify(Arg.Any<string>(), Arg.Any<string>());
            await occupancyAlertServiceBuilder.AlertsStore.DidNotReceive().WriteMeasure(Arg.Any<AlertMeasure>());
        }
    }
}
