using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public class MotionSensorOccupancyEvaluatorStrategy : OccupancyEvaluatorStrategyBase
    {
        public const string Evaluator = "motion";

        public MotionSensorOccupancyEvaluatorStrategy(IOptions<OccupancyDevicesConfiguration> options)
            : base(options, Evaluator)
        {
        }

        public override bool IsOccupancyDetected(string topic, string payload)
        {
            var data = DeserializePayload<Payload>(payload);
            return data.Occupancy ?? throw new System.Exception($"Payload {payload} could not be parsed");
        }

        class Payload
        {
            public bool? Occupancy { get; set; }
        }
    }
}
