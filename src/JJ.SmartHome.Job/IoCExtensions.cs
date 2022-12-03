using JJ.SmartHome.Core;
using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.Core.Alerts.Queries;
using JJ.SmartHome.Core.EnvSensors;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Notifications;
using JJ.SmartHome.Db;
using JJ.SmartHome.Job.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .ConfigureDb(configuration)
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
                .AddTransient<IEnvSensorsService, EnvSensorsService>()
                .AddTransient<ILastFiredAlertQuery, LastFiredAlertQuery>()
                .AddSingleton<AlertStatusProvider>();
        }

        private static IServiceCollection ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<InfluxDbOptions>(configuration.GetSection("InfluxDB"))
                .AddTransient<InfluxDBClientProvider>()
                .AddTransient<IFluxQueryBuilder>(c => new FluxQueryBuilder(
                    c.GetService<IOptions<InfluxDbOptions>>(),
                    c.GetService<InfluxDBClientProvider>().Get()
                ))
                .AddTransient<IEnvSensorsStore, EnvSensorsStore>(c => 
                    new EnvSensorsStore(
                        c.GetService<IOptions<InfluxDbOptions>>(),
                        c.GetService<InfluxDBClientProvider>().Get()
                    ))
                .AddTransient<IAlertsStore, AlertsStore>(c => 
                    new AlertsStore(
                        c.GetService<IOptions<InfluxDbOptions>>(),
                        c.GetService<InfluxDBClientProvider>().Get()
                    ));
        }

        private static IServiceCollection ConfigureHost(this IServiceCollection services)
        {
            return services
                .AddBackgroundService<IOccupancyAlertService>()
                .AddBackgroundService<IAlarmStatusService>()
                .AddBackgroundService<IEnvSensorsService>()
                ;
        }

        private static IServiceCollection AddBackgroundService<T>(this IServiceCollection services) where T : IBackgroundService
        {
            return services
                .AddHostedService<BackgroundHostedService<T>>((c) => new BackgroundHostedService<T>(c.GetService<T>()));
        }
        

    }
}
