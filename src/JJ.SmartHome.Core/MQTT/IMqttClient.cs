﻿using System;
using System.Threading.Tasks;
using MQTTnet;

namespace JJ.SmartHome.Core.MQTT
{
    public interface IMqttClient
    {
        Task Connect(string clientIdPrefix, Func<Task> connected = null, Func<Task> disconnected = null);
        Task Close();
        void Dispose();
        Task Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> handler);
        Task Publish(string topic, string payload);
        Task Publish<T>(string topic, T payload);
    }
}