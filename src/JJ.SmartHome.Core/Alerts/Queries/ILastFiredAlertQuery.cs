using System.Threading.Tasks;
using JJ.SmartHome.Db.Entities;

namespace JJ.SmartHome.Core.Alerts.Queries
{
    public interface ILastFiredAlertQuery
    {
        Task<AlertMeasure> CheckLastFiredAlert();
    }
}
