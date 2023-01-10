using System;
using System.Linq;
using System.Threading.Tasks;
using JJ.SmartHome.Core.EnvSensors.Dto;
using JJ.SmartHome.Core.MQTT;
using JJ.SmartHome.Db;
using JJ.SmartHome.Db.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Core.EnvSensors
{
    public class EnvSensorsBackgroundService : MqttListenerService<EnvSensorsEvent>
    {
        private readonly IEnvSensorsStore _envSensorsStore;

        public EnvSensorsBackgroundService(
            IMqttClient mqttClient,
            IOptions<EnvSensorsOptions> options,
            IEnvSensorsStore envSensorsStore,
            ILogger<EnvSensorsBackgroundService> logger) 
            : base(mqttClient, options.Value.EnvSensorsTopic, "EnvSensors", logger)
        {
            _envSensorsStore = envSensorsStore;
        }

        protected override async Task HandleMessage(string topic, EnvSensorsEvent message)
        {
            var location = topic.Split('/').Last();
            if (message.Temperature.HasValue)
            {
                try
                {
                    _logger.LogDebug($"Storing temperature measure");
                    await _envSensorsStore.WriteMeasure(new Temperature
                    {
                        Location = location,
                        Value = message.Temperature.Value,
                        Time = message.Time
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error storing temperature measure");
                }
            }

            if (message.Pressure.HasValue)
            {
                try
                {
                    _logger.LogDebug($"Storing pressure measure");
                    await _envSensorsStore.WriteMeasure(new Pressure
                    {
                        Location = location,
                        Value = message.Pressure.Value,
                        Time = message.Time
                    });

                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error storing pressure measure");
                }
            }

            if (message.Humidity.HasValue)
            {
                try
                {
                    _logger.LogDebug($"Storing humidity measure");
                    await _envSensorsStore.WriteMeasure(new Humidity
                    {
                        Location = location,
                        Value = message.Humidity.Value,
                        Time = message.Time
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
