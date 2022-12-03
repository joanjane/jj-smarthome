using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Flux.Domain;

namespace JJ.SmartHome.Db
{
    public interface IFluxQueryBuilder
    {
        IFluxQueryBuilder From(string bucket = null);
        IFluxQueryBuilder Range(DateTimeOffset startRange, DateTimeOffset stopRange);
        IFluxQueryBuilder Range(DateTimeOffset startRange, string stopRange = "now()");
        IFluxQueryBuilder Range(string startRange, string stopRange = "now()");
        IFluxQueryBuilder FilterMeasurement(string measurement);
        IFluxQueryBuilder Group(string[] columns);
        IFluxQueryBuilder Aggregate(string aggregateFn);
        IFluxQueryBuilder AggregateWindow(string aggregateFn, string windowSize);
        Task<List<FluxTable>> Query();
    }
}