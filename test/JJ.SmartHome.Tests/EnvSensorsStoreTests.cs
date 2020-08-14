﻿using JJ.SmartHome.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests
{
    public class EnvSensorsStoreTests
    {
        [Fact]
        public async Task GivenNewTempMeasure_WhenWritten_ThenCanBeRetrieved()
        {
            var options = BuildOptions();
            var influxManagement = new InfluxManagement(options);
            var authToken = await influxManagement.Setup();
            options = BuildOptions(authToken);

            var envSensorsStore = BuildEnvSensorsStore(options);

            DateTime utcNow = DateTime.UtcNow;
            var expectedTemp = 25D + (1D / (double)utcNow.Millisecond);
            const string location = "kitchen";

            await envSensorsStore.WriteMeasure(new Db.Entities.Temperature
            {
                Location = location,
                Value = expectedTemp,
                Time = utcNow
            });

            var tempMeasures = await envSensorsStore.QueryMeasure(
                "temperature",
                utcNow.AddSeconds(-1).ToString("o"),
                utcNow.AddSeconds(1).ToString("o")
            );

            Assert.NotEmpty(tempMeasures);

            var actualTemp = tempMeasures
                .SingleOrDefault(t => t.Records.Any(c => c.GetValueByKey("location") as string == location))
                ?.Records
                .Select(fluxRecord => fluxRecord.GetValue())
                .SingleOrDefault();

            Assert.Equal(expectedTemp, actualTemp);
        }

        private static EnvSensorsStore BuildEnvSensorsStore(IOptions<InfluxDbOptions> options)
        {
            var influxDBClientProvider = new InfluxDBClientProvider(options);
            var envSensorsStore = new EnvSensorsStore(options, influxDBClientProvider.Get());
            return envSensorsStore;
        }

        private static IOptions<InfluxDbOptions> BuildOptions(string token = null)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .Build();

            var config = configuration.GetSection("InfluxDB").Get<InfluxDbOptions>();
            if (!string.IsNullOrEmpty(token))
            {
                config.Token = token;
            }
            return Options.Create(config);
        }
    }
}
