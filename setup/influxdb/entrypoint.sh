#!/bin/sh

ls /root/bin/
/root/bin/influxd &
sleep 10s && (sh /root/.influxdbsetup/setup.sh) &
sleep infinity