using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.Alerts.Queries;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Db;
using JJ.SmartHome.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.Alerts
{
    public class OccupancyAlertBackgroundService : MqttListenerService<AqaraOccupancySensorEvent>
    {
        private readonly string _occupancyAlertTopic;
        private readonly IAlertsStore _alertsStore;
        private readonly ILastFiredAlertQuery _lastFiredAlertQuery;
        private readonly IAlertNotifier _alertNotifier;
        private readonly AlertStatusProvider _alertStatusProvider;

        public OccupancyAlertBackgroundService(
            IMqttClient mqttClient,
            IOptions<AlertsOptions> options,
            IAlertsStore alertsStore,
            ILastFiredAlertQuery lastFiredAlertQuery,
            IAlertNotifier alertNotifier,
            AlertStatusProvider alertStatusProvider,
            ILogger<OccupancyAlertBackgroundService> logger)
            : base(mqttClient, options.Value.OccupancyTopic, "Occupancy", logger)
        {
            _occupancyAlertTopic = options.Value.OccupancyAlertTopic;
            _alertsStore = alertsStore;
            _lastFiredAlertQuery = lastFiredAlertQuery;
            _alertNotifier = alertNotifier;
            _alertStatusProvider = alertStatusProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CheckLastFiredAlert();
            await base.ExecuteAsync(stoppingToken);
        }

        protected override async Task HandleMessage(string topic, AqaraOccupancySensorEvent message)
        {
            if (message.Occupancy)
            {
                _logger.LogInformation($"Occupancy detected {DateTime.Now.ToString("s")}");
                if (_alertStatusProvider.ShouldRaiseAlert())
                {
                    _logger.LogInformation($"Notifying alert. Last fired alert '{_alertStatusProvider.GetLastFiredAlert():s}'");
                    var timestamp = _alertStatusProvider.RaiseAlert();
                    var location = topic.Split('/').LastOrDefault() ?? topic;

                    await _alertNotifier.Notify($"[JJ.Alert.Occupancy] {topic}", $"Occupancy was detected on {location}.<br />Payload: <pre>{message}</pre>");
                    
                    await _mqttClient.Publish(_occupancyAlertTopic, new OccupancyAlertEvent
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
            else
            {
                _logger.LogDebug($"No occupancy detected");
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
