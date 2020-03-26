using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Notifications;
using Microsoft.Extensions.Configuration;
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
                .ConfigureAppConfiguration((builder) =>
                {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<MqttClientOptions>(ctx.Configuration.GetSection("MQTT"));
                    services.Configure<EmailOptions>(ctx.Configuration.GetSection("SMTP"));

                    services.AddTransient<IMqttClient, MqttClient>();
                    services.AddTransient<SmtpClientFactory>();
                    services.AddTransient<IAlertNotifier, EmailAlertNotifier>(c => new EmailAlertNotifier(
                        c.GetRequiredService<IOptions<EmailOptions>>(),
                        c.GetRequiredService<SmtpClientFactory>().Build()
                    ));

                    services.AddHostedService<OccupancyAlertHostedService>();
                });
    }
}
