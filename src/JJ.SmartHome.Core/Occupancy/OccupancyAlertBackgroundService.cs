using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JJ.SmartHome.Core.Alarm;
using JJ.SmartHome.Core.Alarm.Queries;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Occupancy.Dto.Alerts;
using JJ.SmartHome.Core.Occupancy.Evaluation;
using JJ.SmartHome.Db;
using JJ.SmartHome.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace JJ.SmartHome.Core.Occupancy
{
    public class OccupancyAlertBackgroundService : MqttListenerService
    {
        private const int AlertGracePeriodDelaySeconds = 10;
        private readonly OccupancyOptions _options;
        private readonly IAlertsStore _alertsStore;
        private readonly ILastFiredAlertQuery _lastFiredAlertQuery;
        private readonly IAlertNotifier _alertNotifier;
        private readonly AlarmStatusProvider _alertStatusProvider;
        private readonly IOccupancyEvaluator _occupancyEvaluator;

        public OccupancyAlertBackgroundService(
            IMqttClient mqttClient,
            IOptions<OccupancyOptions> options,
            IAlertsStore alertsStore,
            ILastFiredAlertQuery lastFiredAlertQuery,
            IAlertNotifier alertNotifier,
            AlarmStatusProvider alertStatusProvider,
            IOccupancyEvaluator occupancyEvaluator,
            ILogger<OccupancyAlertBackgroundService> logger)
            : base(mqttClient, options.Value.OccupancyTopic, "Occupancy", logger)
        {
            _options = options.Value;
            _alertsStore = alertsStore;
            _lastFiredAlertQuery = lastFiredAlertQuery;
            _alertNotifier = alertNotifier;
            _alertStatusProvider = alertStatusProvider;
            _occupancyEvaluator = occupancyEvaluator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CheckLastFiredAlert();
            await base.ExecuteAsync(stoppingToken);
        }

        protected override async Task HandleMqttMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var topic = message.ApplicationMessage.Topic;
            var location = topic.Split('/').LastOrDefault() ?? topic;
            var payload = GetMessagePayload(message);
            var occupancy = _occupancyEvaluator.IsOccupancyDetected(topic, payload);

            if (!occupancy)
            {
                _logger.LogDebug($"No occupancy detected");
                return;
            }

            _logger.LogInformation($"Occupancy detected {DateTime.Now.ToString("s")}\n{payload}");
            await Task.Delay(AlertGracePeriodDelaySeconds * 1000);
            
            if (_alertStatusProvider.ShouldRaiseAlert(_options.SnoozePeriodAfterAlerting))
            {
                _logger.LogInformation($"Notifying alert. Last fired alert '{_alertStatusProvider.GetLastFiredAlert():s}'");
                var timestamp = _alertStatusProvider.RaiseAlert();

                await _alertNotifier.Notify($"[JJ.Alert.Occupancy] {topic}", $"Occupancy was detected on {location}.<br />Payload: <pre>{payload}</pre>");

                await _mqttClient.Publish(_options.OccupancyAlertTopic, new OccupancyAlertEvent
                {
                    Fired = true,
                    Location = location,
                    Timestamp = timestamp
                });

                await _alertsStore.WriteMeasure(new Db.Entities.AlertMeasure
                {
                    Location = location,
                    Reason = "occupancy",
                    Value = 1,
                    Time = timestamp
                });
            }
        }

        private async Task CheckLastFiredAlert()
        {
            var lastFiredTime = await _lastFiredAlertQuery.CheckLastFiredAlert();
            if (lastFiredTime != null)
            {
                _logger.LogInformation($"Last fired alert was {lastFiredTime.Time:s}");
                _alertStatusProvider.SetLastFiredAlert(lastFiredTime.Time);
            }
        }
    }
}
