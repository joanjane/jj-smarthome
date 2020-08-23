#!/bin/bash
docker-compose \
-f ./docker-compose.arm.yml \
-f ./docker-compose.mqtt.yml \
-f ./docker-compose.db-v1.yml \
up -d jj-smarthome-job jj-smarthome-sensehat zigbee2mqtt mosquitto influxdb_v1 chronograf