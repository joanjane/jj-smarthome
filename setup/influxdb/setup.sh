#!/bin/bash

/root/bin/influx setup -o "$InfluxDB__Organization" -b "$InfluxDB__Bucket" -u "$InfluxDB__User" -p "$InfluxDB__Password" -r "$InfluxDB__Retention" -t "$InfluxDB__Token" -f