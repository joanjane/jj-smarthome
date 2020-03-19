using System;
using System.Threading.Tasks;
using MQTTnet;

namespace JJ.SmartHome.Core
{
    public interface IMqttClient
    {
        Task Connect();
        void Dispose();
        Task Subscribe(string topic, Action<MqttApplicationMessageReceivedEventArgs> handler);
    }
}