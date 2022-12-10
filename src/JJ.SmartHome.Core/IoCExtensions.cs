using JJ.SmartHome.Core.Alarm;
using JJ.SmartHome.Core.Alarm.Queries;
using JJ.SmartHome.Core.EnvSensors;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Occupancy;
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
                .Configure<AlarmOptions>(configuration.GetSection("Alerts"))
                .Configure<OccupancyOptions>(configuration.GetSection("Occupancy"))
                .Configure<EnvSensorsOptions>(configuration.GetSection("EnvSensors"))
                .AddTransient<ILastFiredAlertQuery, LastFiredAlertQuery>()
                .AddSingleton<AlarmStatusProvider>()
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
