docker compose `
-f ./docker-compose.app.yml `
-f ./docker-compose.app.override.yml `
-f ./docker-compose.fakes.yml `
-f ./docker-compose.mqtt.yml `
-f ./docker-compose.db-v1.yml `
--env-file .development.env `
up -d jj-smarthome-webapi jj-smarthome-sensehat