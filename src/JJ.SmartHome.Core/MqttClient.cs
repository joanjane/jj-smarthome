using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace JJ.SmartHome.Core
{
    public class MqttClient : IDisposable, IMqttClient
    {
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttClient> _logger;
        private IManagedMqttClient _client;

        public MqttClient(IOptions<MqttClientOptions> options, ILogger<MqttClient> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }

        public async Task Connect(Action connected = null, Action disconnected = null)
        {
            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                //.WithCredentials(_options.User, _options.Password)
                .WithTcpServer(_options.URI, _options.Port)
                .WithCleanSession();

            var options = _options.Secure
              ? messageBuilder
                .WithTls()
                .Build()
              : messageBuilder
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
              .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
              .WithClientOptions(options)
              .Build();

            _client = new MqttFactory()
                .CreateManagedMqttClient();
            _client.UseConnectedHandler(e =>
            {
                _logger.LogInformation("Connected from MQTT Broker.");
                connected?.Invoke();
            })
            .UseDisconnectedHandler(e =>
            {
                _logger.LogWarning(e.Exception, $"Disconnected from MQTT Broker.");
                disconnected?.Invoke();
            });
            await _client.StartAsync(managedOptions);
        }

        public async Task Subscribe(string topic, Action<MqttApplicationMessageReceivedEventArgs> handler)
        {
            await _client.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build());

            _client.UseApplicationMessageReceivedHandler(handler);
        }

        public async Task Publish(string topic, string payload)
        {
            await _client.PublishAsync(new ManagedMqttApplicationMessageBuilder()
                .WithApplicationMessage(new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build()
                )
                .Build());
        }

        public Task Publish<T>(string topic, T payload) => Publish(topic, JsonSerializer.Serialize(payload));

        public Task Close()
        {
            return _client.StopAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client?.Dispose();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
