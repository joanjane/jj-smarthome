using JJ.SmartHome.Db;
using JJ.SmartHome.Db.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alarm.Queries
{
    public class LastFiredAlertQuery : ILastFiredAlertQuery
    {
        private readonly IFluxQueryBuilder _fluxQueryBuilder;

        public LastFiredAlertQuery(IFluxQueryBuilder fluxQueryBuilder)
        {
            _fluxQueryBuilder = fluxQueryBuilder;
        }

        public async Task<AlertMeasure> CheckLastFiredAlert()
        {
            var lastFiredAlert = await _fluxQueryBuilder
                .From()
                .Range(DateTimeOffset.UtcNow.AddDays(-1))
                .FilterMeasurement("alert")
                .Group(new[] { "_measurement" })
                .Aggregate("last()")
                .Query();

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
