using JJ.SmartHome.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<MqttClientOptions>(ctx.Configuration.GetSection("MQTT"));
                    services.AddTransient<IMqttClient, MqttClient>();
                    
                    services.AddHostedService<OccupancyAlertHostedService>();
                });
    }
}
