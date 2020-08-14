#!/bin/sh

influx export all --org $JJ_ORG -t $JJ_TOKEN > ./export.yml