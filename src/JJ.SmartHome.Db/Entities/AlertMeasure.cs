using System;
using InfluxDB.Client.Core;

namespace JJ.SmartHome.Db.Entities
{
    [Measurement("alert")]
    public class AlertMeasure
    {
        [Column("location", IsTag = true)] public string Location { get; set; }
        [Column("reason", IsTag = true)] public string Reason { get; set; }

        [Column("value")] public double Value { get; set; }

        [Column(IsTimestamp = true)] public DateTimeOffset Time;
    }
}