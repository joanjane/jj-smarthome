using JJ.SmartHome.Core.Alerts;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Job
{
    public class OccupancyAlertHostedService : BackgroundService
    {
        private readonly IOccupancyAlertService _occupancyAlertService;

        public OccupancyAlertHostedService(IOccupancyAlertService occupancyAlertService)
        {
            _occupancyAlertService = occupancyAlertService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _occupancyAlertService.HandleOccupancyAlerts(stoppingToken);
        }
    }
}
