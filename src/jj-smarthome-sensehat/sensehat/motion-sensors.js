const env = require('../env');

function remoteSensorsFactory() {
  const { RemoteMotionSensors } = require('pi-sense-hat-remote-simulator/cjs/client');
  const { nodeWebSocketFactory } = require('pi-sense-hat-remote-simulator/cjs/client/node-web-socket-provider');
  const instance = new RemoteMotionSensors(nodeWebSocketFactory, env.SERVER_URI, env.DEVICE);

  return instance;
}

function sensorsFactory() {
  const { MotionSensors } = require('pi-sense-hat-library/cjs');
  const instance = new MotionSensors();

  return instance;
}

module.exports.createMotionSensors = () => {
  return env.MODE === 'simulator' ?
    remoteSensorsFactory() :
    sensorsFactory();
};
