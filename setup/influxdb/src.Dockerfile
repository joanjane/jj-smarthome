FROM golang:1.13.0-buster

ARG InfluxDB__Version=2.0.0-beta.16
ARG InfluxDB__Arch=arm64

WORKDIR /src/influx

RUN apt-get update && apt-get install git curl nano build-essential bzr protobuf-compiler libprotobuf-dev -y
RUN apt-get install gcc-5-multilib-arm-linux-gnueabihf -y

# COPY ./install-rust.sh .
# RUN chmod +x *.sh; ./install-rust.sh

COPY ./install-build-deps.sh .
RUN chmod +x *.sh; ./install-build-deps.sh

COPY ./build-influx.sh .
RUN chmod +x *.sh; ./build-influx.sh

COPY . .
EXPOSE 9999
CMD [ "bash", "./entrypoint.sh" ]