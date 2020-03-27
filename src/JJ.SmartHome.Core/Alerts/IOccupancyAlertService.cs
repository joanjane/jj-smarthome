using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alerts
{
    public interface IOccupancyAlertService
    {
        Task HandleOccupancyAlerts(CancellationToken stoppingToken);
    }
}