using System.Collections.Generic;

namespace JJ.SmartHome.Notifications
{
    public class EmailOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public IEnumerable<string> NotificationAddresses { get; set; }
    }
}
