using System;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlertStatusProvider
    {
        private readonly AlertsOptions _options;
        private object LockObject = new object();
        private DateTimeOffset? LastFiredAlert { get; set; }
        private AlarmStatus Status = AlarmStatus.Armed;

        public AlertStatusProvider(IOptions<AlertsOptions> options)
        {
            _options = options.Value;
        }

        public bool ShouldRaiseAlert()
        {
            return Status == AlarmStatus.Armed && (!LastFiredAlert.HasValue || LastFiredAlert.Value.Add(_options.SnoozePeriodAfterAlerting) < DateTime.UtcNow);
        }

        public DateTimeOffset RaiseAlert()
        {
            lock (LockObject)
            {
                LastFiredAlert = DateTimeOffset.UtcNow;
            }

            return LastFiredAlert.Value;
        }

        public void SetAlertStatus(AlarmStatus status)
        {
            if (!Enum.IsDefined(status))
            {
                throw new ArgumentException("Invalid alarm status to set", nameof(status));
            }
            Status = status;
        }

        public AlarmStatus GetAlertStatus()
        {
            return Status;
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

    public enum AlarmStatus
    {
        Armed = 1,
        Disarmed
    }
}
