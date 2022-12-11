namespace JJ.SmartHome.Core.Occupancy.Evaluation
{
    public interface IOccupancyEvaluatorStrategy : IOccupancyEvaluator
    {
        bool ShouldHandle(string topic);
    }
}
