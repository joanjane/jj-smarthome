using JJ.SmartHome.Core.Alerts;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests
{
    public class AlertStatusProviderTests
    {
        [Fact]
        public async Task GivenNoAlerts_WhenAlertThrown_ThenShouldRaiseIt()
        {
            var options = BuildOptions();
            var sut = new AlertStatusProvider(options);

            // Initially, alert can be raised
            sut.SetAlertStatus("lock");
            Assert.True(sut.ShouldRaiseAlert());
            sut.RaiseAlert();
            
            // Don't allow to raise more alerts until snooze period has passed
            Assert.False(sut.ShouldRaiseAlert());
            
            // wait for snooze period
            await Task.Delay((int)options.Value.SnoozePeriodAfterAlerting.TotalMilliseconds);
            
            // raising alerts should be allowed again
            Assert.True(sut.ShouldRaiseAlert());
        }

        [Fact]
        public void GivenUnlockedAlarm_WhenAlertThrownAndLocked_ThenShouldRaiseIt()
        {
            var options = BuildOptions();
            var sut = new AlertStatusProvider(options);

            // Initially, alert is off
            sut.SetAlertStatus("unlock");

            // Don't allow to raise alerts
            Assert.False(sut.ShouldRaiseAlert());
        }

        private static IOptions<AlertsOptions> BuildOptions()
        {
            return Options.Create(new AlertsOptions
            {
                OccupancyTopic = "none", 
                StatusTopic = "none",
                SnoozePeriodAfterAlerting = TimeSpan.FromSeconds(2)
            });
        }
    }
}
