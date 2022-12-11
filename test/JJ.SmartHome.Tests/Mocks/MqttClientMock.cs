using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JJ.SmartHome.Core.MQTT;
using MQTTnet;

namespace JJ.SmartHome.Tests.Mocks
{
    public class MqttClientMock : IMqttClient
    {
        public Dictionary<string, Func<MqttApplicationMessageReceivedEventArgs, Task>> Subscribers = new Dictionary<string, Func<MqttApplicationMessageReceivedEventArgs, Task>>();
        private TaskCompletionSource<bool> SubscribersReady = new TaskCompletionSource<bool>();

        public virtual Task Close()
        {
            return Task.CompletedTask;
        }

        public virtual async Task Connect(string clientIdPrefix, Func<Task> connected = null, Func<Task> disconnected = null)
        {
            await connected();
        }

        public virtual void Dispose() { }

        public virtual async Task Publish(string topic, string payload)
        {
            await SubscribersReady.Task;

            if (Subscribers.ContainsKey(topic))
            {
                await Subscribers[topic](
                    new MQTTnet.MqttApplicationMessageReceivedEventArgs(
                        "test",
                        new MqttApplicationMessage()
                        {
                            Topic = topic,
                            Payload = Encoding.UTF8.GetBytes(payload.ToCharArray())
                        }
                    )
                );
            }
        }

        public virtual Task Publish<T>(string topic, T payload)
        {
            return Publish(topic, JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }

        public virtual Task Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
        {
            Subscribers[topic] = handler;
            SubscribersReady.SetResult(true);
            return Task.CompletedTask;
        }
    }
}
