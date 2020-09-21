const {
  createDisplay,
  createJoystick,
  createEnvironmentSensors
} = require('./sensehat');

const { MqttClient } = require('./mqtt/mqtt-client');
const { App } = require('./app');

const display = createDisplay();
const joystick = createJoystick();
const environmentSensors = createEnvironmentSensors();
const mqttClient = new MqttClient();

const app = new App(display, joystick, environmentSensors, mqttClient);
app.init();

const shutdown = (signal) => {
  console.log(`${signal} signal received.`);
  app.destroy();
  process.exit(0);
};

process.on('SIGINT', shutdown);

