#!/bin/bash

git clone --depth 1 --branch $InfluxDB__Version https://github.com/influxdata/influxdb.git
cd influxdb

make

ls -la
ls -la **/*

cp influx /usr/local/bin/
cp influxd /usr/local/bin/