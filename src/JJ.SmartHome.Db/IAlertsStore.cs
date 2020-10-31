using System.Threading.Tasks;
using JJ.SmartHome.Db.Entities;
using System.Collections.Generic;
using InfluxDB.Client.Core.Flux.Domain;

namespace JJ.SmartHome.Db
{
    public interface IAlertsStore
    {
        Task<List<FluxTable>> QueryMeasure(string measure, string startRange, string stopRange = "now()", string aggregateFn = "sum", string[] group = null, string windowSize = null);
        Task WriteMeasure(AlertMeasure measure);
    }
}