using JJ.SmartHome.Core.Alerts;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Job.BackgroundServices
{
    public class AlarmStatusHostedService : BackgroundService
    {
        private readonly IAlarmStatusService _service;

        public AlarmStatusHostedService(IAlarmStatusService occupancyAlertService)
        {
            _service = occupancyAlertService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _service.Run(stoppingToken);
        }
    }
}
