using System.Collections.Generic;

namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public class OccupancyDevicesConfiguration
    {
        public IList<Device> Devices { get; set; }

        public class Device
        {
            public string Type { get; set; }
            public string Topic { get; set; }
            public string Evaluator { get; set; }
        }
    }

    public class DeviceType
    {
        public const string Occupancy = "occupancy";
    }
}
