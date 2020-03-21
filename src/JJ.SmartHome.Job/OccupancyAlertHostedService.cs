using JJ.SmartHome.Core;
using JJ.SmartHome.Job.Dto;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Job
{
    public class OccupancyAlertHostedService : BackgroundService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<OccupancyAlertHostedService> _logger;
        private readonly MqttClientOptions _options;

        public OccupancyAlertHostedService(
            IMqttClient mqttClient,
            IOptions<MqttClientOptions> options,
            ILogger<OccupancyAlertHostedService> logger)
        {
            _mqttClient = mqttClient;
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Start {nameof(OccupancyAlertHostedService)}");
            await _mqttClient.Connect(async () =>
            {
                _logger.LogInformation($"Subscribing to {_options.Topic}");
                await _mqttClient.Subscribe(_options.Topic, HandleMessage);
            });

            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            _logger.LogInformation($"End {nameof(OccupancyAlertHostedService)}");
        }

        protected Task HandleMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
            _logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {payload}");
            var messageEvent = JsonSerializer.Deserialize<AqaraOccupancySensorEvent>(payload);
            if (messageEvent.Occupancy)
            {
                _logger.LogInformation($"Occupancy detected {DateTime.Now.ToString("s")}");
            }
            else
            {
                _logger.LogInformation($"No occupancy detected");
            }
            return Task.CompletedTask;
        }
    }
}
