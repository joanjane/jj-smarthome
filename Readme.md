# JJ SmartHome
A pet project to build my own smarthome network using 3rd party tools.

## Development
1. Create .env file copying `.development.env` in setup folder.
2. Run required dependencies using docker-compose (MQTT broker and SMTP server) with `./run-dev-services.ps1` on setup folder.
3. Read received emails on http://localhost:8025/
4. To simulate an occupancy detected event, run `dotnet test --filter TestCategory=DeviceOccupiedSimulation`

## JJ.SmartHome.WebApi
This is the main application that listens MQTT topics for occupancy sensors and weather. 

To start using occupancy sensors (motion or magnet sensors), you must create a devices.json file based on devices.sample.json you can find on project.

### jj-smarthome-sensehat
This is an application that emits environmental data to an MQTT topic and can be consumed on the SmartHome API. 

For development, use simulator mode. Rename file `/src/jj-smarthome-sensehat/.sample.env` to `.env` if debugging locally, or use setup scripts for docker.
Use [pi-sense-hat-remote-simulator](https://joanjane.github.io/pi-sense-hat-remote-simulator/) web simulator.

## Deployment
1. On /setup folder, set variables on `.env` file from sample file `.sample.env`
2. If a new version is deployed, run `./build.sh` script
3. Run `RUN_SERVICES=1 RUN_WEBAPI=1 RUN_SENSEHAT=1 ./run.sh` to launch all services in background

## Configuration
TODO

## Used devices:
* Zigbee usb dongle: CC2531 USB stick.
* Occupancy sensor: Xiaomi Aqara RTCGQ11LM
* Raspberry Pi as a Hub for hosting the system
* Raspberry Pi Sense Hat to monitor environmental information and using joystick for commands.
* [Aqara temperature, humidity and pressure sensor](https://www.zigbee2mqtt.io/devices/WSDCGQ11LM.html#xiaomi-wsdcgq11lm)
* [Aqara human body movement and illuminance sensor](https://www.zigbee2mqtt.io/devices/RTCGQ11LM.html#xiaomi-rtcgq11lm)
* [Aqara door & window contact sensor](https://www.zigbee2mqtt.io/devices/MCCGQ11LM.html#xiaomi-mccgq11lm)
