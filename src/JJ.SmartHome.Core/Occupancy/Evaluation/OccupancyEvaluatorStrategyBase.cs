using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public abstract class OccupancyEvaluatorStrategyBase : IOccupancyEvaluatorStrategy
    {
        protected readonly string _evaluator;
        protected readonly IOptionsSnapshot<OccupancyDevicesConfiguration> _options;

        public OccupancyEvaluatorStrategyBase(IOptionsSnapshot<OccupancyDevicesConfiguration> options, string evaluator)
        {
            _options = options;
            _evaluator = evaluator;
        }

        public bool ShouldHandle(string topic)
        {
            return _options.Value.Devices.Any(d => d.Topic == topic && d.Evaluator == _evaluator);
        }

        public abstract bool IsOccupancyDetected(string topic, string payload);

        protected static T DeserializePayload<T>(string payload)
        {
            return JsonSerializer.Deserialize<T>(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
