namespace JJ.SmartHome.Db.Entities
{
    public interface IEnvSensorMeasure
    {
        string Location { get; set; }
        double Value { get; set; }
    }
}