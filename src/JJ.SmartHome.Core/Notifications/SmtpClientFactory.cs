using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace JJ.SmartHome.Core.Notifications
{
    public class SmtpClientFactory
    {
        private readonly EmailOptions _options;

        public SmtpClientFactory(IOptions<EmailOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public SmtpClient Build()
        {
            return new SmtpClient()
            {
                Host = _options.Host,
                Port = _options.Port,
                EnableSsl = _options.Ssl,
                Credentials = new NetworkCredential(_options.Username, _options.Password)
            };
        }
    }
}
