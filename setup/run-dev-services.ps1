docker-compose `
-f ./docker-compose.x86.yml `
-f ./docker-compose.fakes.yml `
-f ./docker-compose.mqtt.yml `
-f ./docker-compose.db-v1.yml `
up -d mosquitto mailhog jj-ws-server influxdb_v1 chronograf