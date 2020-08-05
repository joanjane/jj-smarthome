#!/bin/bash
docker build -t jj-smarthome-sensehat .
docker run --privileged --rm -it --env-file=./.env jj-smarthome-sensehat

