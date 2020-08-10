const dotenv = require('dotenv');
dotenv.config();

module.exports = {
  MODE: process.env.MODE,
  SERVER_URI: process.env.SERVER_URI || 'ws://localhost:8080',
  DEVICE: process.env.DEVICE || 'jj-smarthome-sensehat',
  MQTT_URI: process.env.MQTT_URI,
  MQTT_ALARM_TOPIC: process.env.MQTT_ALARM_TOPIC || 'alarm/status',
  MQTT_ENV_SENSORS_TOPIC: process.env.MQTT_ENV_SENSORS_TOPIC || 'env-sensors/sensehat'
};