# JJ SmartHome
A pet project to build my own smarthome network using 3rd party tools.

## Development
1. Run required dependencies using docker-compose (MQTT broker and SMTP server) with `./run-dev-containers`
2. Read received emails on http://localhost:8025/
3. To simulate an occupancy detected event, run `dotnet test --filter TestCategory=DeviceOccupiedSimulation`

## Deployment
1. On /setup folder, set variables on `.env` file from sample file `.env.sample`
2. If a new version is deployed, run `docker-compose build`
3. Run `docker-compose up -d` to launch all services in background

## Used devices:
* Zigbee usb dongle: CC2531 USB stick.
* Occupancy sensor: Xiaomi Aqara RTCGQ11LM
* Raspberry Pi as a Hub for hosting the system