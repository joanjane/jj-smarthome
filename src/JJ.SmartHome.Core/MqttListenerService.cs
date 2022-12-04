using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JJ.SmartHome.Core.Extensions;
using JJ.SmartHome.Core.MQTT;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace JJ.SmartHome.Core
{
    public abstract class MqttListenerService : BackgroundService
    {
        protected readonly IMqttClient _mqttClient;
        protected readonly string _clientPrefix;
        protected readonly string _topic;
        protected readonly ILogger _logger;

        public MqttListenerService(
            IMqttClient mqttClient,
            string topic,
            string clientPrefix,
            ILogger logger)
        {
            _mqttClient = mqttClient;
            _topic = topic;
            _clientPrefix = clientPrefix;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Start {GetType().Name}");

            var task = Task.Run(async () => 
                await _mqttClient.Connect(_clientPrefix, async () =>
                {
                    _logger.LogInformation($"Subscribing to {_topic}");
                    await _mqttClient.Subscribe(_topic, HandleMqttMessage);
                })
            , stoppingToken);

            stoppingToken.LoopUntilCancelled();
            _logger.LogInformation($"End {GetType().Name}");

            return task;
        }

        protected virtual T DeserializeMessage<T>(MqttApplicationMessageReceivedEventArgs message)
        {
            var payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload);

            _logger.LogInformation($"Topic {message.ApplicationMessage.Topic}. Message {payload}");

            return JsonSerializer.Deserialize<T>(payload);
        }

        protected abstract Task HandleMqttMessage(MqttApplicationMessageReceivedEventArgs message);
    }

    public abstract class MqttListenerService<T> : MqttListenerService
    {
        public MqttListenerService(
            IMqttClient mqttClient,
            string topic,
            string clientPrefix,
            ILogger logger) : base(mqttClient, topic, clientPrefix, logger) { }

        protected override Task HandleMqttMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var messageEvent = DeserializeMessage<T>(message);
            return HandleMessage(message.ApplicationMessage.Topic, messageEvent);
        }

        protected abstract Task HandleMessage(string topic, T message);
    }
}
