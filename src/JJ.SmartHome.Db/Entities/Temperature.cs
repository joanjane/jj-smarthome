using System;
using InfluxDB.Client.Core;

namespace JJ.SmartHome.Db.Entities
{

    [Measurement("temperature")]
    public class Temperature : IEnvSensorMeasure
    {
        [Column("location", IsTag = true)] public string Location { get; set; }

        [Column("value")] public double Value { get; set; }

        [Column(IsTimestamp = true)] public DateTime Time;
    }
}