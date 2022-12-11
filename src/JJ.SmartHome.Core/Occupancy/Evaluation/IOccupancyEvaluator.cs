namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public interface IOccupancyEvaluator
    {
        bool IsOccupancyDetected(string topic, string payload);
    }
}
