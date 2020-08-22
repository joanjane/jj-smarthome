FROM golang:1.13.0-buster

ARG InfluxDB__Version=2.0.0-beta.16
ARG InfluxDB__Arch=arm64

WORKDIR /src/influx

# COPY ./install-rust.sh .
# RUN sh ./install-rust.sh

COPY ./install-build-deps.sh .
RUN sh ./install-build-deps.sh

COPY ./build-influx.sh .
RUN sh ./build-influx.sh

COPY . .
EXPOSE 9999
CMD [ "sh", "./entrypoint.sh" ]