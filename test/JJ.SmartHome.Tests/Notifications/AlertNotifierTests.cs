using JJ.SmartHome.Core.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Xunit;

namespace JJ.SmartHome.Tests.Alerts.Notifications
{
    public class AlertNotifierTests
    {
        [Fact]
        public async Task WhenNotify_ThenSuccess()
        {
            var alertNotifier = BuildEmailAlertNotifier();
            await alertNotifier.Notify("test message", "test content");
        }

        private static EmailAlertNotifier BuildEmailAlertNotifier()
        {
            var configuration = Utils.ConfigBuilder.Build();
            
            var options = Options.Create(configuration.GetSection("SMTP").Get<EmailOptions>());

            var logger = LoggerFactory.Create(c => c.AddConsole()).CreateLogger<EmailAlertNotifier>();
            return new EmailAlertNotifier(
                options,
                new SmtpClientFactory(options),
                logger);
        }
    }
}
