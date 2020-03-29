using System;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlertsOptions
    {
        public string OccupancyTopic { get; set; }
        public TimeSpan SnoozePeriodAfterAlerting { get; set; }
    }
}
