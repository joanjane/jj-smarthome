using System;
using System.Threading.Tasks;
using MQTTnet;

namespace JJ.SmartHome.Core
{
    public interface IMqttClient
    {
        Task Connect(Action connected = null, Action disconnected = null);
        Task Close();
        void Dispose();
        Task Subscribe(string topic, Action<MqttApplicationMessageReceivedEventArgs> handler);
        Task Publish(string topic, string payload);
        Task Publish<T>(string topic, T payload);
    }
}