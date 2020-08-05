const env = require('../env');

function remoteDisplayClientFactory() {
  const { RemoteDisplay } = require('pi-sense-hat-remote-simulator/cjs/client');
  const { nodeWebSocketFactory } = require('pi-sense-hat-remote-simulator/cjs/client/node-web-socket-provider');
  const display = new RemoteDisplay(nodeWebSocketFactory, env.SERVER_URI, env.DEVICE);

  return display;
}

function displayFactory() {
  const { Display } = require('pi-sense-hat-library/cjs');
  const enableLogs = true;
  const display = new Display(enableLogs);

  return display;
}

module.exports.createDisplay = () => {
  console.log(`Display mode ${env.MODE}`);
  return env.MODE === 'simulator' ?
    remoteDisplayClientFactory() :
    displayFactory();
};
