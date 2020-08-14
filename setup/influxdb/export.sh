#!/bin/sh

influx export all --org $InfluxDB__Organization -t $InfluxDB__Token > /root/.influxdbsetup/export.yml