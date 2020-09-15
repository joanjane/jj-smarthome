using JJ.SmartHome.Core.EnvSensors.Dto;
using JJ.SmartHome.Core.Extensions;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Db;
using JJ.SmartHome.Db.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.SmartHome.Core.EnvSensors
{
    public class EnvSensorsService : IEnvSensorsService
    {
        private readonly IMqttClient _mqttClient;
        private readonly IEnvSensorsStore _envSensorsStore;
        private readonly ILogger<EnvSensorsService> _logger;
        private readonly EnvSensorsOptions _options;

        public EnvSensorsService(
            IMqttClient mqttClient,
            IOptions<EnvSensorsOptions> options,
            IEnvSensorsStore envSensorsStore,
            ILogger<EnvSensorsService> logger)
        {
            _mqttClient = mqttClient;
            _envSensorsStore = envSensorsStore;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Run(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Start {nameof(EnvSensorsService)}");
            await _mqttClient.Connect("EnvSensors", async () =>
            {
                _logger.LogInformation($"Subscribing to {_options.EnvSensorsSenseHatTopic}");
                await _mqttClient.Subscribe(_options.EnvSensorsSenseHatTopic, HandleMessage);
            });

            stoppingToken.LoopUntilCancelled();

            _logger.LogInformation($"End {nameof(EnvSensorsService)}");
        }

        protected async Task HandleMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
            _logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {payload}");

            var location = message.ApplicationMessage.Topic.Split('/').Last();
            var messageEvent = JsonSerializer.Deserialize<EnvSensorsEvent>(payload);

            if (messageEvent.Temperature.HasValue)
            {
                try
                {
                    _logger.LogDebug($"Storing temperature measure");
                    await _envSensorsStore.WriteMeasure(new Temperature
                    {
                        Location = location,
                        Value = messageEvent.Temperature.Value,
                        Time = messageEvent.Time
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error storing temperature measure");
                }
            }

            if (messageEvent.Pressure.HasValue)
            {
                try
                {
                    _logger.LogDebug($"Storing pressure measure");
                    await _envSensorsStore.WriteMeasure(new Pressure
                    {
                        Location = location,
                        Value = messageEvent.Pressure.Value,
                        Time = messageEvent.Time
                    });

                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error storing pressure measure");
                }
            }

            if (messageEvent.Humidity.HasValue)
            {
                try
                {
                    _logger.LogDebug($"Storing humidity measure");
                    await _envSensorsStore.WriteMeasure(new Humidity
                    {
                        Location = location,
                        Value = messageEvent.Humidity.Value,
                        Time = messageEvent.Time
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error storing humidity measure");
                }
            }
            
            _logger.LogDebug($"Stored measures");
        }
    }
}
