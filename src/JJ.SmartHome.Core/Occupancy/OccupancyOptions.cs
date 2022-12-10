using System;

namespace JJ.SmartHome.Core.Occupancy
{
    public class OccupancyOptions
    {
        public string OccupancyTopic { get; set; }
        public string OccupancyAlertTopic { get; set; }
        public TimeSpan SnoozePeriodAfterAlerting { get; set; }
    }
}
