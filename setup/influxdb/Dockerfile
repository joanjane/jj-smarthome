FROM debian:buster-slim
ARG InfluxDB__Version=2.0.0-beta.16
ARG InfluxDB__Arch=arm64

WORKDIR /tmp
RUN apt-get update && apt-get install curl -y
RUN curl -L https://dl.influxdata.com/influxdb/releases/influxdb_${InfluxDB__Version}_linux_${InfluxDB__Arch}.tar.gz --output influxdb_${InfluxDB__Version}_linux_${InfluxDB__Arch}.tar.gz

RUN tar xvzf ./influxdb_${InfluxDB__Version}_linux_${InfluxDB__Arch}.tar.gz

RUN mkdir /root/bin
RUN mv ./influxdb_${InfluxDB__Version}_linux_${InfluxDB__Arch}/influx /root/bin
RUN mv ./influxdb_${InfluxDB__Version}_linux_${InfluxDB__Arch}/influxd /root/bin
RUN chmod +x /root/bin/*
RUN ls -la /root/bin

ENV PATH="/root/bin:${PATH}"

WORKDIR /root/.influxdbsetup
COPY . .
RUN chmod +x ./*.sh

CMD [ "./entrypoint.sh" ]