namespace JJ.SmartHome.Core.Alarm.Dto
{
    public record SetAlarmStatusEvent
    {
        public AlarmStatus Status { get; set; }
    }
}
