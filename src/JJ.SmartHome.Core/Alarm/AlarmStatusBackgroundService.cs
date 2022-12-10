using System.Threading.Tasks;
using JJ.SmartHome.Core.Alarm.Dto;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alarm
{
    public class AlarmStatusBackgroundService : MqttListenerService<SetAlarmStatusEvent>
    {
        private readonly AlarmStatusProvider _alertStatusProvider;

        public AlarmStatusBackgroundService(
            IMqttClient mqttClient,
            IOptions<AlarmOptions> options,
            AlarmStatusProvider alertStatusProvider,
            ILogger<AlarmStatusBackgroundService> logger)
            : base(mqttClient, options.Value.ChangeStatusTopic, "AlarmStatus", logger)
        {
            _alertStatusProvider = alertStatusProvider;
        }

        protected override Task HandleMessage(string topic, SetAlarmStatusEvent message)
        {
            _alertStatusProvider.SetAlertStatus(message.Status);
            _logger.LogInformation($"Set alarm to {message.Status}");

            return Task.CompletedTask;
        }
    }
}
