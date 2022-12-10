using System;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlertsOptions
    {
        public string OccupancyTopic { get; set; }
        public string OccupancyAlertTopic { get; set; }
        public TimeSpan SnoozePeriodAfterAlerting { get; set; }
        public string StatusTopic { get; set; }
    }
}
