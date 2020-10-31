using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using JJ.SmartHome.Db.Entities;
using System.Collections.Generic;
using InfluxDB.Client.Core.Flux.Domain;
using System.Linq;

namespace JJ.SmartHome.Db
{
    public class AlertsStore : IAlertsStore
    {
        private readonly InfluxDbOptions _options;
        private readonly InfluxDBClient _influxDBClient;

        public AlertsStore(IOptions<InfluxDbOptions> options, InfluxDBClient influxDBClient)
        {
            _options = options.Value;
            _influxDBClient = influxDBClient;
        }

        public async Task WriteMeasure(AlertMeasure measure)
        {
            await _influxDBClient
                .GetWriteApiAsync()
                .WriteMeasurementAsync(
                    _options.Bucket,
                    _options.Organization,
                    WritePrecision.Ms,
                    measure);
        }

        public async Task<List<FluxTable>> QueryMeasure(string measure, string startRange, string stopRange = "now()", string aggregateFn = "sum", string[] group = null, string windowSize = null)
        {
            var query = $@"
                from(bucket:""{_options.Bucket}"")
                |> range(start: {startRange}, stop: {stopRange})
                |> filter(fn: (r) => r._measurement == ""{measure}"")
                ";

            if (group != null)
            {
                var groups = string.Join(",", group.Select(g => '"' + g + '"'));
                query += $@"
                |> group(columns: [{groups}])";
            }
            
            if (string.IsNullOrEmpty(windowSize))
            {
                query += $@"
                    |> {aggregateFn}";
            }
            else
            {
                query += $@"
                    |> aggregateWindow(every: {windowSize}, fn: {aggregateFn})
                    |> fill(usePrevious: true)";
            }

            return await _influxDBClient
                .GetQueryApi()
                .QueryAsync(query);
        }

    }
}