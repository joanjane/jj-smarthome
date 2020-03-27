using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace JJ.SmartHome.Job
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builder) =>
                {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.ConfigureContainer(ctx.Configuration);
                });
    }
}
