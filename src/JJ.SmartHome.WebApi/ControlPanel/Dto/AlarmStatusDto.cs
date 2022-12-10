using System;
using JJ.SmartHome.Core.Alarm;

namespace JJ.SmartHome.WebApi.ControlPanel.Dto
{
    public class AlarmStatusDto
    {
        public AlarmStatus Status { get; set; }
    }

    public class AlarmStatusDetailsDto : AlarmStatusDto
    {
        public DateTimeOffset? LastFiredAlert { get; set; }
    }
}
