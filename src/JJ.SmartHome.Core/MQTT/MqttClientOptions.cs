namespace JJ.SmartHome.Core.MQTT
{
    public class MqttClientOptions
    {
        public string URI { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 1883;
        public bool Secure { get; set; }
        public string Topic { get; set; }
    }
}