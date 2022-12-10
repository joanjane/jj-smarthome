const dotenv = require('dotenv');
dotenv.config();

module.exports = {
  MODE: process.env.MODE,
  SERVER_URI: process.env.SERVER_URI || 'ws://localhost:8080',
  DEVICE: process.env.DEVICE || 'jj-smarthome-sensehat',
  MQTT_URI: process.env.MQTT_URI,
  MQTT_CLIENT_ID: process.env.MQTT_CLIENT_ID || 'jj-smarthome-sensehat',
  MQTT_ALARM_TOPIC: process.env.MQTT_ALARM_TOPIC || 'alarm/status',
  MQTT_OCCUPANCY_ALERT_TOPIC: process.env.MQTT_OCCUPANCY_ALERT_TOPIC || 'alert/occupancy',
  MQTT_ENV_SENSORS_TOPIC: process.env.MQTT_ENV_SENSORS_TOPIC || 'env-sensors/sensehat',
  MQTT_PERMIT_JOIN_TOPIC: process.env.MQTT_PERMIT_JOIN_TOPIC || 'zigbee2mqtt/bridge/config/permit_join'
};