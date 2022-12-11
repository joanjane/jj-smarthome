using System.Collections.Generic;
using System.Linq;

namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public class OccupancyEvaluator : IOccupancyEvaluator
    {
        private readonly IEnumerable<IOccupancyEvaluatorStrategy> _occupancyEvaluationStrategies;

        public OccupancyEvaluator(IEnumerable<IOccupancyEvaluatorStrategy> occupancyEvaluationStrategies)
        {
            _occupancyEvaluationStrategies = occupancyEvaluationStrategies;
        }

        public bool IsOccupancyDetected(string topic, string payload)
        {
            var strategy = _occupancyEvaluationStrategies.FirstOrDefault(s => s.ShouldHandle(topic))
                ?? throw new System.Exception($"Couldn't find an occupancy evaluator for {topic}");
            
            return strategy.IsOccupancyDetected(topic, payload);
        }
    }
}
