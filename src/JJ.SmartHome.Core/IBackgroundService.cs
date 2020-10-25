using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core
{
    public interface IBackgroundService
    {
        Task Run(CancellationToken stoppingToken);
    }
}