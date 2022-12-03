using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using JJ.SmartHome.Db.Entities;

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
    }
}