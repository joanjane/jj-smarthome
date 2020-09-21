# JJ SmartHome
A pet project to build my own smarthome network using 3rd party tools.

## Development
1. Create .env file copying `.development.env` in setup folder.
2. Run required dependencies using docker-compose (MQTT broker and SMTP server) with `./run-dev-containers` on setup folder.
3. Read received emails on http://localhost:8025/
4. To simulate an occupancy detected event, run `dotnet test --filter TestCategory=DeviceOccupiedSimulation`

### jj-smarthome-sensehat
For development, use simulator mode. Rename file `/src/jj-smarthome-sensehat/.sample.env` to `.env` if debugging locally, or use setup scripts for docker.
Use [pi-sense-hat-remote-simulator](https://joanjane.github.io/pi-sense-hat-remote-simulator/) web simulator.

## Deployment
1. On /setup folder, set variables on `.env` file from sample file `.sample.env`
2. If a new version is deployed, run `./build.sh` script
3. Run `./run.sh` to launch all services in background

## Used devices:
* Zigbee usb dongle: CC2531 USB stick.
* Occupancy sensor: Xiaomi Aqara RTCGQ11LM
* Raspberry Pi as a Hub for hosting the system
* Raspberry Pi Sense Hat to monitor environmental information and using joystick for commands.