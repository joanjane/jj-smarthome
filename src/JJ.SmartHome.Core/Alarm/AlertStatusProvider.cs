using System;

namespace JJ.SmartHome.Core.Alarm
{
    public class AlarmStatusProvider
    {
        private object LockObject = new object();
        private DateTimeOffset? LastFiredAlert { get; set; }
        private AlarmStatus Status = AlarmStatus.Armed;

        public bool ShouldRaiseAlert(TimeSpan snoozePeriodAfterAlerting)
        {
            return Status == AlarmStatus.Armed && (!LastFiredAlert.HasValue || LastFiredAlert.Value.Add(snoozePeriodAfterAlerting) < DateTime.UtcNow);
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

        public AlarmStatus GetAlarmStatus()
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
