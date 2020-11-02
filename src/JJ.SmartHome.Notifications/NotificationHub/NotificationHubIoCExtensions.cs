using JJ.SmartHome.Notifications.NotificationHub.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JJ.SmartHome.Notifications.NotificationHub
{
    public static class NotificationHubIoCExtensions
    {
        public static void ConfigureNotificationHub(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<INotificationService, NotificationHubService>();

            services.AddOptions<NotificationHubOptions>()
                .Configure(configuration.GetSection("NotificationHub").Bind)
                .ValidateDataAnnotations();
        }
    }
}