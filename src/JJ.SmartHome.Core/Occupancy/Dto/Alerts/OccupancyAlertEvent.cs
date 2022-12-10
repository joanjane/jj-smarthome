using System;

namespace JJ.SmartHome.Core.Occupancy.Dto.Alerts
{
    public record OccupancyAlertEvent
    {
        public string Location { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public bool Fired { get; set; }
    }
}
