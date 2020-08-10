docker-compose `
-f ./docker-compose.x86.yml `
-f ./docker-compose.fakes.yml `
-f ./docker-compose.mqtt.yml `
# -f ./docker-compose.db.yml `
up -d mosquitto mailhog jj-ws-server # influxdb chronograf grafana