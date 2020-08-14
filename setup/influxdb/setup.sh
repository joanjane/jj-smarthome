#!/bin/sh

influx setup -o "$JJ_ORG" -b "$JJ_BUCKET" -u "$JJ_USER" -p "$JJ_PASSWORD" -r "$JJ_RETENTION" -t "$JJ_TOKEN" -f