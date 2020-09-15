﻿using JJ.SmartHome.Core.Alerts.Dto;
using JJ.SmartHome.Core.Extensions;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Core.Notifications;
using JJ.SmartHome.Db;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Alerts
{
    public class OccupancyAlertService : IOccupancyAlertService
    {
        private readonly IMqttClient _mqttClient;
        private readonly IAlertsStore _alertsStore;
        private readonly IAlertNotifier _alertNotifier;
        private readonly AlertStatusProvider _alertStatusProvider;
        private readonly ILogger<OccupancyAlertService> _logger;
        private readonly AlertsOptions _options;

        public OccupancyAlertService(
            IMqttClient mqttClient,
            IOptions<AlertsOptions> options,
            IAlertsStore alertsStore,
            IAlertNotifier alertNotifier,
            AlertStatusProvider alertStatusProvider,
            ILogger<OccupancyAlertService> logger)
        {
            _mqttClient = mqttClient;
            _alertsStore = alertsStore;
            _alertNotifier = alertNotifier;
            _alertStatusProvider = alertStatusProvider;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Run(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Start {nameof(OccupancyAlertService)}");
            await _mqttClient.Connect("Occupancy", async () =>
            {
                _logger.LogInformation($"Subscribing to {_options.OccupancyTopic}");
                await _mqttClient.Subscribe(_options.OccupancyTopic, HandleMessage);
            });

            stoppingToken.LoopUntilCancelled();

            _logger.LogInformation($"End {nameof(OccupancyAlertService)}");
        }

        protected async Task HandleMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
            _logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {payload}");

            var messageEvent = JsonSerializer.Deserialize<AqaraOccupancySensorEvent>(payload);
            if (messageEvent.Occupancy)
            {
                _logger.LogInformation($"Occupancy detected {DateTime.Now.ToString("s")}");
                if (_alertStatusProvider.ShouldRaiseAlert())
                {
                    _logger.LogInformation($"Notifying alert. Last fired alert '{_alertStatusProvider.GetLastFiredAlert():s}'");
                    _alertStatusProvider.RaiseAlert();
                    await _alertNotifier.Notify($"[JJ.Alert.Occupancy] {message.ApplicationMessage.Topic}", $"Occupancy was detected.<br />Payload: <pre>{payload}</pre>");
                    await _alertsStore.WriteMeasure(new Db.Entities.AlertMeasure {
                        Location = message.ApplicationMessage.Topic,
                        Reason = "occupancy",
                        Value = 1,
                        Time = _alertStatusProvider.GetLastFiredAlert() ?? DateTime.UtcNow
                    });
                }
            }
            else
            {
                _logger.LogDebug($"No occupancy detected");
            }
        }
    }
}
