using System;
using InfluxDB.Client.Core;

namespace JJ.SmartHome.Db.Entities
{
    [Measurement("humidity")]
    public class Humidity : IEnvSensorMeasure
    {
        [Column("location", IsTag = true)] public string Location { get; set; }

        [Column("value")] public double Value { get; set; }

        [Column(IsTimestamp = true)] public DateTime Time;
    }
}