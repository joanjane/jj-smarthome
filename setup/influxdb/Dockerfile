FROM debian:buster-slim
ENV VERSION=2.0.0-beta.16

WORKDIR /tmp
RUN apt-get update && apt-get install curl -y
RUN curl -L https://dl.influxdata.com/influxdb/releases/influxdb_${VERSION}_linux_arm64.tar.gz --output influxdb_${VERSION}_linux_arm64.tar.gz

RUN tar xvzf ./influxdb_${VERSION}_linux_arm64.tar.gz

RUN cp ./influxdb_${VERSION}_linux_arm64/influx /usr/local/bin/influx
RUN cp ./influxdb_${VERSION}_linux_arm64/influxd /usr/local/bin/influxd

ENV PATH="/usr/local/bin:${PATH}"

WORKDIR /root/.influxdbsetup
COPY . .

CMD [ "influxd" ]