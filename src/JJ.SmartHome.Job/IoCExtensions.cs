using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace JJ.SmartHome.Job
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .ConfigureMqtt(configuration)
                .ConfigureMailing(configuration)
                .ConfigureCore(configuration)
                .ConfigureHost();
        }

        private static IServiceCollection ConfigureMqtt(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<MqttClientOptions>(configuration.GetSection("MQTT"))
                .AddTransient<IMqttClient, MqttClient>();
        }

        private static IServiceCollection ConfigureMailing(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<EmailOptions>(configuration.GetSection("SMTP"))
                .AddTransient<SmtpClientFactory>()
                .AddTransient<IAlertNotifier, EmailAlertNotifier>(c => new EmailAlertNotifier(
                    c.GetRequiredService<IOptions<EmailOptions>>(),
                    c.GetRequiredService<SmtpClientFactory>().Build(),
                    c.GetRequiredService<ILogger<EmailAlertNotifier>>()
                ));
        }

        private static IServiceCollection ConfigureCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<AlertsOptions>(configuration.GetSection("Alerts"))
                .AddTransient<IOccupancyAlertService, OccupancyAlertService>()
                .AddSingleton<AlertStatusProvider>();
        }

        private static IServiceCollection ConfigureHost(this IServiceCollection services)
        {
            return services.AddHostedService<OccupancyAlertHostedService>();
        }
    }
}
