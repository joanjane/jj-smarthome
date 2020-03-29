using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Notifications
{
    public class EmailAlertNotifier : IAlertNotifier
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<EmailAlertNotifier> _logger;
        private readonly EmailOptions _options;

        public EmailAlertNotifier(
            IOptions<EmailOptions> options, 
            SmtpClient smtpClient, 
            ILogger<EmailAlertNotifier> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _smtpClient = smtpClient;
            _logger = logger;
        }


        public async Task Notify(string title, string content)
        {
            var emailSentTask = new TaskCompletionSource<bool>();

            using (var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.Sender),
                Subject = title,
                Body = content
            })
            {
                _options.NotificationAddresses.ToList().ForEach(c => mailMessage.To.Add(new MailAddress(c)));
                _smtpClient.SendCompleted += (sender, eventArgs) =>
                {
                    if (eventArgs.Error != null)
                    {
                        _logger.LogError(eventArgs.Error, $"Error when sending an email to server {_options.Host}");
                        emailSentTask.SetException(eventArgs.Error);
                    }
                    else if (eventArgs.Cancelled)
                    {
                        _logger.LogWarning("Email send was cancelled"); 
                        emailSentTask.SetCanceled();
                    }
                    else
                    {
                        _logger.LogDebug("Email sent successfuly");
                        emailSentTask.SetResult(true);
                    }
                };
                
                await _smtpClient.SendMailAsync(mailMessage);
                await emailSentTask.Task;
            }
        }
    }
}
