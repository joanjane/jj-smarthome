using JJ.SmartHome.Core.Alerts;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Job.BackgroundServices
{
    public class BackgroundHostedService<T> : BackgroundService where T : IBackgroundService
    {
        private readonly IBackgroundService _service;

        public BackgroundHostedService(T service)
        {
            _service = service;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () =>
            {
                await _service.Run(stoppingToken);
            }, stoppingToken);
            
            return Task.CompletedTask;
        }
    }
}
