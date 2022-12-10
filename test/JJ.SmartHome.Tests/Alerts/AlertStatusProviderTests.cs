using System;
using System.Threading.Tasks;
using JJ.SmartHome.Core.Alarm;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts
{
    public class AlertStatusProviderTests
    {
        [Fact]
        [Trait("TestCategory", "Unit")]
        public async Task GivenNoAlerts_WhenAlertThrown_ThenShouldRaiseIt()
        {
            var snoozePeriodAfterAlerting = TimeSpan.FromSeconds(5);
            var sut = new AlarmStatusProvider();

            // Initially, alert can be raised
            sut.SetAlertStatus(AlarmStatus.Armed);
            Assert.True(sut.ShouldRaiseAlert(snoozePeriodAfterAlerting));
            sut.RaiseAlert();
            
            // Don't allow to raise more alerts until snooze period has passed
            Assert.False(sut.ShouldRaiseAlert(snoozePeriodAfterAlerting));
            
            // wait for snooze period
            await Task.Delay((int)snoozePeriodAfterAlerting.TotalMilliseconds);
            
            // raising alerts should be allowed again
            Assert.True(sut.ShouldRaiseAlert(snoozePeriodAfterAlerting));
        }

        [Fact]
        [Trait("TestCategory", "Unit")]
        public void GivenUnlockedAlarm_WhenAlertThrownAndLocked_ThenShouldRaiseIt()
        {
            var snoozePeriodAfterAlerting = TimeSpan.FromSeconds(5);
            var sut = new AlarmStatusProvider();

            // Initially, alert is off
            sut.SetAlertStatus(AlarmStatus.Disarmed);

            // Don't allow to raise alerts
            Assert.False(sut.ShouldRaiseAlert(snoozePeriodAfterAlerting));
        }
    }
}
