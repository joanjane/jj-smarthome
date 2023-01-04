using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public class MagnetSensorOccupancyEvaluatorStrategy : OccupancyEvaluatorStrategyBase
    {
        public const string Evaluator = "magnet";

        public MagnetSensorOccupancyEvaluatorStrategy(IOptions<OccupancyDevicesConfiguration> options)
            : base(options, Evaluator)
        {
        }

        public override bool IsOccupancyDetected(string topic, string payload)
        {
            var data = DeserializePayload<Payload>(payload);
            return !data.Contact ?? throw new System.Exception($"Payload {payload} could not be parsed");
        }

        class Payload
        {
            public bool? Contact { get; set; }
        }
    }
}
