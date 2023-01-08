const env = require('./env');
const { renderAnimation } = require('./sensehat/animations/utils');

const envSensorsCheckMinutes = 5;
const alarmStatus = {
  armed: 1,
  disarmed: 2
};
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
      this.display.showMessage('Connected', 0.05, '#3c8cd7');

      this.interval = setInterval(
        () => this.checkEnvironmentStatus(),
        envSensorsCheckMinutes * 60 * 1000);

      this.checkEnvironmentStatus();
      this.setAlarmControls();
      this.setPermitJoinControls();
      this.listenAlertEvents();
    });
  }

  listenAlertEvents() {
    this.mqttClient.subscribe(env.MQTT_OCCUPANCY_ALERT_TOPIC)
      .then(() => console.log('Subscribed to topic', env.MQTT_OCCUPANCY_ALERT_TOPIC))
      .catch(e => console.error('Could not subscribe to topic', env.MQTT_OCCUPANCY_ALERT_TOPIC, e));
      
    this.mqttClient.onMessage(env.MQTT_OCCUPANCY_ALERT_TOPIC, (topic, occupancyEvent) => {
      console.log('Received occuppancy event', occupancyEvent);
      if (occupancyEvent.fired && new Date(occupancyEvent.timestamp) > getRelativeDateByMinutes(5)) {
        renderAnimation(this.display, 4, require('./sensehat/animations/alert.json'));
      } else {
        console.log('Discarded occuppancy event');
      }
    });
  }

  setAlarmControls() {
    this.joystick.on('press', (e) => {
      console.log('joystick press ' + e);

      if (e === 'up') {
        this.setAlarmOn();
      } else if (e === 'down') {
        this.setAlarmOff();
      }
    });
  }

  setAlarmOn() {
    let countdown = 15;
    renderAnimation(this.display, countdown, require('./sensehat/animations/lock.json'))
      .then(() => {
        this.mqttClient.publish(env.MQTT_ALARM_TOPIC, { status: alarmStatus.armed });
      });
  }
  
  setAlarmOff() {
    this.mqttClient.publish(env.MQTT_ALARM_TOPIC, { status: alarmStatus.disarmed });
    let countdown = 3;
    renderAnimation(this.display, countdown, require('./sensehat/animations/unlock.json'));
  }

  setPermitJoinControls() {
    this.joystick.on('press', (e) => {
      console.log('joystick press ' + e);

      if (e === 'left') {
        this.display.showMessage('PERMIT JOIN', 0.1, '#7ed73a');
        this.mqttClient.publish(env.MQTT_PERMIT_JOIN_TOPIC, 'true');
      } else if (e === 'right') {
        this.display.showMessage('DISABLE JOIN', 0.1, '#d73a49');
        this.mqttClient.publish(env.MQTT_PERMIT_JOIN_TOPIC, 'false');
      }
    });
  }

  checkEnvironmentStatus() {
    console.log('Checking environment status');
    this.environmentSensors.getSensorsStatus().then(envSensorsStatus => {
      const envSensorsEvent = {
        temperature: envSensorsStatus.temperature,
        pressure: envSensorsStatus.pressure,
        humidity: envSensorsStatus.humidity,
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

function getRelativeDateByMinutes(minutes) {
  return new Date(new Date().setMinutes(new Date().getMinutes() + minutes));
}

module.exports.App = App;