#!/bin/bash
set -e
# ex: RUN_SERVICES=1 ./run.sh
# ex: RUN_WEBAPI=1 ./run.sh
# ex: RUN_SENSEHAT=1 ./run.sh

echo "Checking RUN_SERVICES variable eq 1..."
if [[ "$RUN_SERVICES" = "1" ]]
then
    echo "Running services..."
    docker compose \
    -f ./docker-compose.mqtt.yml \
    -f ./docker-compose.db-v1.yml \
    up -d zigbee2mqtt mosquitto influxdb_v1 chronograf
fi

echo "Checking RUN_WEBAPI variable eq 1..."
if [[ "$RUN_WEBAPI" = "1" ]]
then
    if [[ ! -f "./jj-smarthome/devices.json" ]]; then
        echo "You must create ./jj-smarthome/devices.json file. See existing example."
        exit 1
    fi

    echo "Running jj-smarthome-webapi..."
    docker compose -f ./docker-compose.app.yml up --build -d jj-smarthome-webapi
fi

echo "Checking RUN_SENSEHAT variable eq 1..."
if [[ "$RUN_SENSEHAT" = "1" ]]
then
    echo "Running jj-smarthome-sensehat..."
    docker compose -f ./docker-compose.app.yml up --build -d jj-smarthome-sensehat
fi