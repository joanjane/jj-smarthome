using JJ.SmartHome.Notifications.NotificationHub.Dto;
using JJ.SmartHome.Notifications.NotificationHub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JJ.SmartHome.Notifications
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureMailing(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<EmailOptions>(configuration.GetSection("SMTP"))
                .AddTransient<SmtpClientFactory>()
                .AddTransient<IAlertNotifier, EmailAlertNotifier>();
        }

        public static void ConfigureNotificationHub(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<INotificationService, NotificationHubService>();

            services.AddOptions<NotificationHubOptions>()
                .Configure(configuration.GetSection("NotificationHub").Bind)
                .ValidateDataAnnotations();
        }
    }
}
