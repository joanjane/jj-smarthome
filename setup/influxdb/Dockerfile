FROM debian:buster-slim
ENV VERSION=2.0.0-beta.16

WORKDIR /tmp
RUN apt-get update && apt-get install curl -y
RUN curl -L https://dl.influxdata.com/influxdb/releases/influxdb_${VERSION}_linux_arm64.tar.gz --output influxdb_${VERSION}_linux_arm64.tar.gz

RUN tar xvzf ./influxdb_${VERSION}_linux_arm64.tar.gz

RUN mkdir /root/bin
RUN mv ./influxdb_${VERSION}_linux_arm64/influx /root/bin
RUN mv ./influxdb_${VERSION}_linux_arm64/influxd /root/bin
RUN ls /root/bin

ENV PATH="/root/bin:${PATH}"

WORKDIR /root/.influxdbsetup
COPY . .

CMD [ "influxd" ]