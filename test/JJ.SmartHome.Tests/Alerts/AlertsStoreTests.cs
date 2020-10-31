using JJ.SmartHome.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts
{
    public class AlertsStoreTests
    {
        [Fact]
        [Trait("TestCategory", "Integration")]
        [Trait("TestCategory", "Influx")]
        public async Task GivenNewAlert_WhenWritten_ThenCanBeRetrieved()
        {
            var options = BuildOptions();
            var influxManagement = new InfluxManagement(options);
            var authToken = await influxManagement.Setup();
            options = BuildOptions(authToken);

            var alertsStore = BuildAlertsStore(options);

            var utcNow = DateTimeOffset.UtcNow;
            const string location = "hall";

            await alertsStore.WriteMeasure(new Db.Entities.AlertMeasure
            {
                Location = location,
                Value = 1,
                Time = utcNow
            });

            var tempMeasures = await alertsStore.QueryMeasure(
                measure: "alert",
                startRange: utcNow.AddSeconds(-2).ToString("o"),
                stopRange: utcNow.AddSeconds(2).ToString("o"),
                aggregateFn: "max()"
            );

            Assert.NotEmpty(tempMeasures);

            var actualSumAlerts = tempMeasures
                .SingleOrDefault(t => t.Records.Any(c => c.GetValueByKey("location") as string == location))
                ?.Records
                .Select(fluxRecord => fluxRecord.GetValue())
                .SingleOrDefault();

            Assert.Equal(1d, actualSumAlerts);
        }

        private static AlertsStore BuildAlertsStore(IOptions<InfluxDbOptions> options)
        {
            var influxDBClientProvider = new InfluxDBClientProvider(options);
            var alertsStore = new AlertsStore(options, influxDBClientProvider.Get());
            return alertsStore;
        }

        private static IOptions<InfluxDbOptions> BuildOptions(string token = null)
        {
            var configuration = Utils.ConfigBuilder.Build();

            var config = configuration.GetSection("InfluxDB").Get<InfluxDbOptions>();
            if (!string.IsNullOrEmpty(token))
            {
                config.Token = token;
            }
            return Options.Create(config);
        }
    }
}
