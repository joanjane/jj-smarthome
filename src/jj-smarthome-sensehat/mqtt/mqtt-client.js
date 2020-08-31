const mqtt = require('mqtt');
const env = require('../env');

module.exports.MqttClient = class MqttClient {
  constructor() {
    this.client = null;
  }

  connect() {
    return new Promise(resolve => {
      this.client = mqtt.connect(env.MQTT_URI, {
        keepAlive: 90,
        clientId: env.MQTT_CLIENT_ID
      });

      this.client.on('connect', function () {
        console.log('Connected to mqtt');
        resolve();
      });

      this.client.on('close', function () {
        console.log('Closed mqtt');
        resolve();
      });

      this.client.on('disconnect', function () {
        console.log('Disconnected to mqtt');
        resolve();
      });
    })
  }

  publish(topic, content) {
    this.client.publish(topic, JSON.stringify(content));
  }
}