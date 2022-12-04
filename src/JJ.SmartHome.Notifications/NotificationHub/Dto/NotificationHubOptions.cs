using System.ComponentModel.DataAnnotations;

namespace JJ.SmartHome.Notifications.NotificationHub.Dto
{
    public class NotificationHubOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}