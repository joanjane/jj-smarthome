const mqtt = require('mqtt');
const env = require('../env');

module.exports.MqttClient = class MqttClient {
  constructor() {
    this.client = null;
  }

  connect() {
    return new Promise(resolve => {
      this.client = mqtt.connect(env.MQTT_URI);

      this.client.on('connect', function () {
        resolve();
      });
    })
  }

  publish(topic, content) {
    this.client.publish(topic, JSON.stringify(content));
  }
}