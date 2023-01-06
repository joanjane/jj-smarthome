#!/bin/bash
docker compose \
-f ./docker-compose.app.yml \
-f ./docker-compose.mqtt.yml \
-f ./docker-compose.db.yml \
build jj-smarthome-webapi jj-smarthome-sensehat