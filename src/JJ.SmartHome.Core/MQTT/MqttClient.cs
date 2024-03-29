﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace JJ.SmartHome.Core.MQTT
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

        public async Task Connect(string clientIdPrefix, Func<Task> connected = null, Func<Task> disconnected = null)
        {
            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId($"{clientIdPrefix}-{_options.ClientId}")
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(90))
                .WithCredentials(_options.User, _options.Password)
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
            _client.UseConnectedHandler(async e =>
            {
                _logger.LogInformation("Connected from MQTT Broker.");
                if (connected != null)
                {
                    await connected();
                }
            })
            .UseDisconnectedHandler(async e =>
            {
                _logger.LogWarning(e.Exception, $"Disconnected from MQTT Broker.");
                if (disconnected != null)
                {
                    await disconnected();
                }
            });
            await _client.StartAsync(managedOptions);
        }

        public async Task Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
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

        public Task Publish<T>(string topic, T payload) => Publish(topic, JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));

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
