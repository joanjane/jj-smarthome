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
    if (typeof content === 'string') {
      this.client.publish(topic, content);
    } else {
      this.client.publish(topic, JSON.stringify(content));
    }
  }

  subscribe(topic) {
    return new Promise((resolve, reject) => {
      this.client.subscribe(topic, {
        qos: 0
      }, (error, granted) => {
        if (error) {
          reject(error);
        }
        resolve(granted);
      });
    });
  }

  onMessage(subscribingTopic, callback) {
    this.client.on('message', function (topic, payload, packet) {
      if (topic.startsWith(subscribingTopic)) {
        const message = JSON.parse(payload.toString());
        callback(topic, message);
      }
    });
  }
}