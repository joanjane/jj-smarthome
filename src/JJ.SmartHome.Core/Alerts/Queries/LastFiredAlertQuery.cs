using JJ.SmartHome.Db;
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

        public async Task<DateTimeOffset?> CheckLastFiredAlertDate()
        {
            var lastFiredAlert = await _alertsStore.QueryMeasure(
                measure: "alert",
                startRange: DateTimeOffset.UtcNow.AddDays(-1).ToString("o"),
                aggregateFn: "max()"
            );

            var lastFiredTime = lastFiredAlert.FirstOrDefault()?.Records.FirstOrDefault()?.GetTimeInDateTime();
            if (!lastFiredTime.HasValue)
            {
                return null;
            }
            return new DateTimeOffset(lastFiredTime.Value, TimeSpan.Zero);
        }
    }
}
