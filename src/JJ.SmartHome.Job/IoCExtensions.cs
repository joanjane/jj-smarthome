using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Notifications;
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
                .ConfigureCore()
                .ConfigureHost();
        }

        private static IServiceCollection ConfigureMqtt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MqttClientOptions>(configuration.GetSection("MQTT"));
            services.AddTransient<IMqttClient, MqttClient>();
            return services;
        }

        private static IServiceCollection ConfigureMailing(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection("SMTP"));

            services.AddTransient<SmtpClientFactory>();
            services.AddTransient<IAlertNotifier, EmailAlertNotifier>(c => new EmailAlertNotifier(
                c.GetRequiredService<IOptions<EmailOptions>>(),
                c.GetRequiredService<SmtpClientFactory>().Build()
            ));
            return services;
        }

        private static IServiceCollection ConfigureCore(this IServiceCollection services)
        {
            return services.AddTransient<IOccupancyAlertService, OccupancyAlertService>();
        }

        private static IServiceCollection ConfigureHost(this IServiceCollection services)
        {
            return services.AddHostedService<OccupancyAlertHostedService>();
        }
    }
}
