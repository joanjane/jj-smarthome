using System.Threading.Tasks;

namespace JJ.SmartHome.Notifications
{
    public interface IAlertNotifier
    {
        Task Notify(string title, string content);
    }
}