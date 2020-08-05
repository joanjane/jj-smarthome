const env = require('../env');

function remoteJoystickFactory() {
  const { RemoteJoystick } = require('pi-sense-hat-remote-simulator/cjs/client');
  const { nodeWebSocketFactory } = require('pi-sense-hat-remote-simulator/cjs/client/node-web-socket-provider');
  const instance = new RemoteJoystick(nodeWebSocketFactory, env.SERVER_URI, env.DEVICE);

  return instance;
}

function joystickFactory() {
  const { Joystick } = require('pi-sense-hat-library/cjs');
  const instance = new Joystick();

  return instance;
}

module.exports.createJoystick = () => {
  return env.MODE === 'simulator' ?
    remoteJoystickFactory() :
    joystickFactory();
};
