using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alerts
{
    public interface IBackgroundService
    {
        Task Run(CancellationToken stoppingToken);
    }
}