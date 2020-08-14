using System.Linq;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using JJ.SmartHome.Db.Entities;
using System.Collections.Generic;
using InfluxDB.Client.Core.Flux.Domain;

namespace JJ.SmartHome.Db
{
    public class EnvSensorsStore : IEnvSensorsStore
    {
        private const int _expirationDays = 7;
        private readonly InfluxDbOptions _options;
        private readonly InfluxDBClient _influxDBClient;

        public EnvSensorsStore(IOptions<InfluxDbOptions> options, InfluxDBClient influxDBClient)
        {
            _options = options.Value;
            _influxDBClient = influxDBClient;
        }

        public async Task WriteMeasure(IEnvSensorMeasure measure)
        {
            await _influxDBClient
                .GetWriteApiAsync()
                .WriteMeasurementAsync(
                    _options.Bucket,
                    _options.Organization,
                    WritePrecision.Ms,
                    measure);
        }

        public async Task<List<FluxTable>> QueryMeasure(string measure, string startRange, string stopRange = "now", string windowSize = "1h")
        {
            var aggregateFn = "mean";

            var query = $@"
                from(bucket:""{_options.Bucket}"")
                |> range(start: {startRange}, stop: {stopRange})
                |> filter(fn: (r) => r._measurement == ""{measure}"")
                |> aggregateWindow(every: {windowSize}, fn: {aggregateFn})
                |> fill(usePrevious: true)";

            return await _influxDBClient
                .GetQueryApi()
                .QueryAsync(query);
        }

    }
}