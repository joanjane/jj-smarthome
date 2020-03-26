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
        private readonly EmailOptions _options;

        public EmailAlertNotifier(IOptions<EmailOptions> options, SmtpClient smtpClient)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _smtpClient = smtpClient;
        }


        public async Task Notify(string title, string content)
        {
            using (var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.Sender),
                Subject = title,
                Body = content
            })
            {
                _options.NotificationAddresses.ToList().ForEach(c => mailMessage.To.Add(new MailAddress(c)));
                await _smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
