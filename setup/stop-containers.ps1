docker-compose `
-f ./docker-compose.app.yml `
-f ./docker-compose.app.override.yml `
-f ./docker-compose.db-v1.yml `
-f ./docker-compose.fakes.yml `
-f ./docker-compose.mqtt.yml `
-f ./docker-compose.testing.yml `
down