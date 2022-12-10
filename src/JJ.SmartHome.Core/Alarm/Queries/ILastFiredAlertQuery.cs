using System.Threading.Tasks;
using JJ.SmartHome.Db.Entities;

namespace JJ.SmartHome.Core.Alarm.Queries
{
    public interface ILastFiredAlertQuery
    {
        Task<AlertMeasure> CheckLastFiredAlert();
    }
}
