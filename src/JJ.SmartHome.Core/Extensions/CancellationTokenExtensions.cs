using System.Threading;

namespace JJ.SmartHome.Core.Extensions
{
    public static class CancellationTokenExtensions
    {
        public static void LoopUntilCancelled(this CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(1000);
            }

        }
    }
}
