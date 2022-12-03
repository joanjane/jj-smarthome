using JJ.SmartHome.Core.Alerts.Queries;
using JJ.SmartHome.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts
{
    public class LastFiredAlertQueryTests
    {
        [Fact]
        [Trait("TestCategory", "Integration")]
        public async Task GivenAPreviousAlert_WhenGettingLastFiredAlert_ThenReturnDate()
        {
            var options = BuildOptions();
            var influxManagement = new InfluxManagement(options);
            var authToken = await influxManagement.Setup();
            options = BuildOptions(authToken);

            var alertsStore = BuildAlertsStore(options);
            var sut = new LastFiredAlertQuery(BuildFluxQueryBuilder(options));

            var date = DateTimeOffset.UtcNow.AddMinutes(-1);
            var location = "testlastalert" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            await alertsStore.WriteMeasure(new Db.Entities.AlertMeasure
            {
                Location = location,
                Value = 1,
                Time = date
            });

            var lastFiredAlert = await sut.CheckLastFiredAlert();

            Assert.NotNull(lastFiredAlert);
            Assert.Equal(location, lastFiredAlert.Location);
        }

        private static AlertsStore BuildAlertsStore(IOptions<InfluxDbOptions> options)
        {
            var influxDBClientProvider = new InfluxDBClientProvider(options);
            var alertsStore = new AlertsStore(options, influxDBClientProvider.Get());
            return alertsStore;
        }

        private static IFluxQueryBuilder BuildFluxQueryBuilder(IOptions<InfluxDbOptions> options)
        {
            var influxDBClientProvider = new InfluxDBClientProvider(options);
            var envSensorsStore = new FluxQueryBuilder(options, influxDBClientProvider.Get());
            return envSensorsStore;
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
