using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.Core.EnvSensors;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Notifications;
using JJ.SmartHome.Job.BackgroundServices;
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
                .AddTransient<IAlertNotifier, EmailAlertNotifier>();
        }

        private static IServiceCollection ConfigureCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<AlertsOptions>(configuration.GetSection("Alerts"))
                .Configure<EnvSensorsOptions>(configuration.GetSection("EnvSensors"))
                .AddTransient<IOccupancyAlertService, OccupancyAlertService>()
                .AddTransient<IAlarmStatusService, AlarmStatusService>()
                .AddSingleton<AlertStatusProvider>();
        }

        private static IServiceCollection ConfigureHost(this IServiceCollection services)
        {
            return services
                .AddBackgroundService<IOccupancyAlertService>()
                .AddBackgroundService<IAlarmStatusService>();
        }

        private static IServiceCollection AddBackgroundService<T>(this IServiceCollection services) where T : IBackgroundService
        {
            return services
                .AddHostedService<BackgroundHostedService<T>>((c) => new BackgroundHostedService<T>(c.GetService<T>()));
        }

    }
}
