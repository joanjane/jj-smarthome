using System;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlertStatusProvider
    {
        private readonly AlertsOptions _options;
        private object LockObject = new object();
        private DateTimeOffset? LastFiredAlert { get; set; }
        private bool AlarmUnlocked = false;

        public AlertStatusProvider(IOptions<AlertsOptions> options)
        {
            _options = options.Value;
        }

        public bool ShouldRaiseAlert()
        {
            return !AlarmUnlocked && (!LastFiredAlert.HasValue || LastFiredAlert.Value.Add(_options.SnoozePeriodAfterAlerting) < DateTime.UtcNow);
        }

        public void RaiseAlert()
        {
            lock (LockObject)
            {
                LastFiredAlert = DateTimeOffset.UtcNow;
            }
        }

        public void SetAlertStatus(string status)
        {
            if (status == AlarmStatus.Lock)
            {
                AlarmUnlocked = false;
            }
            else if (status == AlarmStatus.Unlock)
            {
                AlarmUnlocked = true;
            }
            else
            {
                throw new ArgumentException("Invalid alarm status to set", nameof(status));
            }
        }

        public DateTimeOffset? GetLastFiredAlert()
        {
            return LastFiredAlert;
        }

        public DateTimeOffset? SetLastFiredAlert(DateTimeOffset time)
        {
            return LastFiredAlert = time;
        }
    }

    public class AlarmStatus
    {
        public const string Lock = "lock";
        public const string Unlock = "unlock";
    }
}
