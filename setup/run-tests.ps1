docker-compose `
-f ./docker-compose.testing.yml `
-f ./docker-compose.fakes.yml `
-f ./docker-compose.mqtt.yml `
-f ./docker-compose.db-v1.yml `
up --build --exit-code-from=jj-smarthome-job-tests `
jj-smarthome-job-tests mosquitto mailhog influxdb_v1