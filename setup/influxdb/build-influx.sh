#!/bin/bash
set -e

export GO111MODULE=on

# modify yarn timeout
yarn config set network-timeout 600000 -g

git clone --depth 1 --branch "v$InfluxDB__Version" https://github.com/influxdata/influxdb.git
cd influxdb

# remove cypress to make it work
cd ui
yarn remove cypress
cd ..

#make
./env go build ./cmd/influxd

ls -la
ls -la bin

#cp ./bin/influx /usr/local/bin/
#cp ./bin/influxd /usr/local/bin/
