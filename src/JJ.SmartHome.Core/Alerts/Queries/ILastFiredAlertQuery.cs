using System;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alerts.Queries
{
    public interface ILastFiredAlertQuery
    {
        Task<DateTimeOffset?> CheckLastFiredAlertDate();
    }
}
