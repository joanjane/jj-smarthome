using JJ.SmartHome.Db;
using JJ.SmartHome.Db.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alerts.Queries
{

    public class LastFiredAlertQuery : ILastFiredAlertQuery
    {
        private readonly IAlertsStore _alertsStore;

        public LastFiredAlertQuery(
            IAlertsStore alertsStore)
        {
            _alertsStore = alertsStore;
        }

        public async Task<AlertMeasure> CheckLastFiredAlert()
        {
            var lastFiredAlert = await _alertsStore.QueryMeasure(
                measure: "alert",
                startRange: DateTimeOffset.UtcNow.AddDays(-1).ToString("o"),
                aggregateFn: "last()",
                group: new[] { "_measure" }
            );

            var lastFiredTime = lastFiredAlert.FirstOrDefault()?.Records.FirstOrDefault();
            if (lastFiredTime == null)
            {
                return null;
            }
            return new AlertMeasure
            {
                Time = lastFiredTime.GetTime()?.ToDateTimeOffset() ?? throw new Exception("Time should be specified"),
                Location = lastFiredTime.GetValueByKey("location") as string,
                Reason = lastFiredTime.GetValueByKey("reason") as string,
            };
        }
    }
}
