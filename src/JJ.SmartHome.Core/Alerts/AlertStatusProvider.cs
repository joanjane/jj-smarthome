using System;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlertStatusProvider
    {
        private readonly AlertsOptions _options;
        private object LockObject = new object();
        private DateTime? LastFiredAlert { get; set; }
        private bool AlarmUnlocked = false;

        public AlertStatusProvider(IOptions<AlertsOptions> options)
        {
            _options = options.Value;
        }

        public bool ShouldRaiseAlert()
        {
            return !AlarmUnlocked || !LastFiredAlert.HasValue || LastFiredAlert.Value.Add(_options.SnoozePeriodAfterAlerting) < DateTime.UtcNow;
        }

        public void RaiseAlert()
        {
            lock (LockObject)
            {
                LastFiredAlert = DateTime.UtcNow;
            }
        }

        public void SetAlertStatus(string status)
        {
            if (status == "lock")
            {
                AlarmUnlocked = false;
            }
            else if (status == "unlock")
            {
                AlarmUnlocked = true;
            }
            else
            {
                throw new ArgumentException("Invalid alarm status to set", nameof(status));
            }
        }
    }
}
