const env = require('./env');

const envSensorsCheckMinutes = 0.2;

class App {
  constructor(display, joystick, environmentSensors, mqttClient) {
    this.display = display;
    this.joystick = joystick;
    this.environmentSensors = environmentSensors;
    this.mqttClient = mqttClient;
  }

  init() {
    console.log('Connecting to modules');
    return Promise.all([
      new Promise((resolve) => this.display.connect(resolve)),
      new Promise((resolve) => this.joystick.connect(resolve)),
      new Promise((resolve) => this.environmentSensors.connect(resolve)),
      this.mqttClient.connect()
    ]).then(() => {
      console.log('Modules connected');

      this.display.clear();
      this.interval = setInterval(
        () => this.checkEnvironmentStatus(),
        envSensorsCheckMinutes * 60 * 1000);

      this.checkEnvironmentStatus();
      this.setAlarmControls();
    });
  }

  setAlarmControls() {
    this.joystick.on('press', (e) => {
      console.log('joystick press ' + e);

      if (e === 'up') {
        this.display.showMessage('Alarm ON', 0.1, '#7ed73a');
        this.mqttClient.publish(env.MQTT_ALARM_TOPIC, { status: 'lock' });
      } else if (e === 'down') {
        this.display.showMessage('Alarm OFF', 0.1, '#d73a49');
        this.mqttClient.publish(env.MQTT_ALARM_TOPIC, { status: 'unlock' });
      }
    });
  }

  checkEnvironmentStatus() {
    console.log('Checking environment status');
    this.environmentSensors.getSensorsStatus().then(envSensorsStatus => {
      const envSensorsEvent = {
        temperature: parseInt(envSensorsStatus.temperature),
        pressure: parseInt(envSensorsStatus.pressure),
        humidity: parseInt(envSensorsStatus.humidity),
        time: new Date().toISOString()
      };

      console.log(envSensorsEvent);

      this.mqttClient.publish(env.MQTT_ENV_SENSORS_TOPIC, envSensorsEvent);
    });
  }

  destroy() {
    if (this.interval) clearInterval(this.interval);
    this.display.clear();
    [this.display, this.joystick, this.environmentSensors].forEach(c => c.close());
  }
}

module.exports.App = App;