const env = require('../env');

function remoteSensorsFactory() {
  const { RemoteEnvironmentSensors } = require('pi-sense-hat-remote-simulator/cjs/client');
  const { nodeWebSocketFactory } = require('pi-sense-hat-remote-simulator/cjs/client/node-web-socket-provider');
  const instance = new RemoteEnvironmentSensors(nodeWebSocketFactory, env.SERVER_URI, env.DEVICE);

  return instance;
}

function sensorsFactory() {
  const { EnvironmentSensors } = require('pi-sense-hat-library/cjs');
  const instance = new EnvironmentSensors();

  return instance;
}

module.exports.createEnvironmentSensors = () => {
  return env.MODE === 'simulator' ?
    remoteSensorsFactory() :
    sensorsFactory();
};
