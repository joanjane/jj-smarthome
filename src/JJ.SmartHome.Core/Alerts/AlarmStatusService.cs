using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.Extensions;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alerts
{
    public class AlarmStatusService : IAlarmStatusService
    {
        private readonly IMqttClient _mqttClient;
        private readonly AlertStatusProvider _alertStatusProvider;
        private readonly ILogger<AlarmStatusService> _logger;
        private readonly AlertsOptions _options;

        public AlarmStatusService(
            IMqttClient mqttClient,
            IOptions<AlertsOptions> options,
            AlertStatusProvider alertStatusProvider,
            ILogger<AlarmStatusService> logger)
        {
            _mqttClient = mqttClient;
            _alertStatusProvider = alertStatusProvider;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Run(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Start {nameof(AlarmStatusService)}");
            await _mqttClient.Connect(async () =>
            {
                _logger.LogInformation($"Subscribing to {_options.StatusTopic}");
                await _mqttClient.Subscribe(_options.StatusTopic, HandleMessage);
            });

            stoppingToken.LoopUntilCancelled();
            
            _logger.LogInformation($"End {nameof(AlarmStatusService)}");
        }

        protected Task HandleMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
            _logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {payload}");

            var messageEvent = JsonSerializer.Deserialize<AlarmStatusEvent>(payload);
            _alertStatusProvider.SetAlertStatus(messageEvent.Status);
            _logger.LogInformation($"Set alarm to {messageEvent.Status}");
            
            return Task.CompletedTask;
        }
    }
}
