#!/bin/bash
docker-compose \
-f ./docker-compose.arm.yml \
-f ./docker-compose.mqtt.yml \
# -f ./docker-compose.db.yml \
build jj-smarthome-job jj-smarthome-sensehat