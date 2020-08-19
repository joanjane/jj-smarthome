#!/bin/bash

git clone --depth 1 --branch "v$InfluxDB__Version" https://github.com/influxdata/influxdb.git
cd influxdb

make

ls -la
ls -la bin

cp ./bin/influx /usr/local/bin/
cp ./bin/influxd /usr/local/bin/