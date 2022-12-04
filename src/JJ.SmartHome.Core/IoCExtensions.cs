using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.Core.Alerts.Queries;
using JJ.SmartHome.Core.EnvSensors;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JJ.SmartHome.Core
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .ConfigureMqtt(configuration)
                .ConfigureServices(configuration)
                .ConfigureBackgroundServices()
                ;
        }

        private static IServiceCollection ConfigureMqtt(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<MqttClientOptions>(configuration.GetSection("MQTT"))
                .AddTransient<IMqttClient, MqttClient>();
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<AlertsOptions>(configuration.GetSection("Alerts"))
                .Configure<EnvSensorsOptions>(configuration.GetSection("EnvSensors"))
                .AddTransient<ILastFiredAlertQuery, LastFiredAlertQuery>()
                .AddSingleton<AlertStatusProvider>()
                .ConfigureBackgroundServices()
                ;
        }

        private static IServiceCollection ConfigureBackgroundServices(this IServiceCollection services)
        {
            return services
                .AddHostedService<OccupancyAlertBackgroundService>()
                .AddHostedService<AlarmStatusBackgroundService>()
                .AddHostedService<EnvSensorsBackgroundService>()
                ;
        }

    }
}
