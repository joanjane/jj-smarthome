using System.Threading.Tasks;
using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlarmStatusBackgroundService : MqttListenerService<AlarmStatusEvent>
    {
        private readonly AlertStatusProvider _alertStatusProvider;

        public AlarmStatusBackgroundService(
            IMqttClient mqttClient,
            IOptions<AlertsOptions> options,
            AlertStatusProvider alertStatusProvider,
            ILogger<AlarmStatusBackgroundService> logger) 
            : base(mqttClient, options.Value.StatusTopic, "AlarmStatus", logger)
        {
            _alertStatusProvider = alertStatusProvider;
        }

        protected override Task HandleMessage(string topic, AlarmStatusEvent message)
        {
            _alertStatusProvider.SetAlertStatus(message.Status);
            _logger.LogInformation($"Set alarm to {message.Status}");
            
            return Task.CompletedTask;
        }
    }
}
