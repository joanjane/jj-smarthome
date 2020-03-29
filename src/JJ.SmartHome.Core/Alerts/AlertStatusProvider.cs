using System;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlertStatusProvider
    {
        private readonly AlertsOptions _options;
        private object LockObject = new object();
        private DateTime? LastFiredAlert { get; set; }

        public AlertStatusProvider(IOptions<AlertsOptions> options)
        {
            _options = options.Value;
        }

        public bool ShouldRaiseAlert()
        {
            return !LastFiredAlert.HasValue || LastFiredAlert.Value.Add(_options.SnoozePeriodAfterAlerting) < DateTime.UtcNow;
        }

        public void RaiseAlert()
        {
            lock(LockObject)
            {
                LastFiredAlert = DateTime.UtcNow;
            }
        }

    }
}
