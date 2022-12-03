using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Db
{
    public class FluxQueryBuilder : IFluxQueryBuilder
    {
        private const string DateFormat = "o";
        private readonly IOptions<InfluxDbOptions> _options;
        private readonly InfluxDBClient _influxDBClient;
        protected string _query;

        public FluxQueryBuilder(IOptions<InfluxDbOptions> options, InfluxDBClient influxDBClient)
        {
            _options = options;
            _influxDBClient = influxDBClient;
        }

        public IFluxQueryBuilder From(string bucket = null)
        {
            if (string.IsNullOrEmpty(bucket))
            {
                bucket = _options.Value.Bucket;
            }
            _query = $@"from(bucket:""{bucket}"")" + "\n";
            return this;
        }
        
        public IFluxQueryBuilder Range(string startRange, string stopRange = "now()")
        {
            _query += $@"|> range(start: {startRange}, stop: {stopRange})" + "\n";
            return this;
        }

        public IFluxQueryBuilder Range(DateTimeOffset startRange, string stopRange = "now()")
        {
            return Range(startRange.ToString(DateFormat), stopRange);
        }
        
        public IFluxQueryBuilder Range(DateTimeOffset startRange, DateTimeOffset stopRange)
        {
            return Range(startRange.ToString(DateFormat), stopRange.ToString(DateFormat));
        }

        public IFluxQueryBuilder FilterMeasurement(string measurement)
        {
            _query += $@"|> filter(fn: (r) => r._measurement == ""{measurement}"")" + "\n";
            return this;
        }

        public IFluxQueryBuilder Group(string[] columns)
        {
            var groups = string.Join(",", columns.Select(g => '"' + g + '"'));
            _query += $@"|> group(columns: [{groups}])" + "\n";
            return this;
        }

        public IFluxQueryBuilder Aggregate(string aggregateFn)
        {
            _query += $@"|> {aggregateFn}" + "\n";
            return this;
        }

        public IFluxQueryBuilder AggregateWindow(string aggregateFn, string windowSize)
        {
            _query +=
                $@" |> aggregateWindow(every: {windowSize}, fn: {aggregateFn})
                    |> fill(usePrevious: true)" + "\n";
            return this;
        }

        public async Task<List<FluxTable>> Query()
        {
            return await _influxDBClient
                .GetQueryApi()
                .QueryAsync(_query);
        }
    }
}