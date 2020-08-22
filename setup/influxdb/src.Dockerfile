FROM ragnaroek/rust-raspberry:1.45.2
# FROM golang:1.13.0-buster

ARG InfluxDB__Version=2.0.0-beta.16
ARG InfluxDB__Arch=arm64

RUN rustup --version && cargo --version && rustc --version

WORKDIR /src/influx

# ENV RUSTUP_HOME=/usr/local/rustup \
#     CARGO_HOME=/usr/local/cargo \
#     GO111MODULE=on

COPY ./install-build-deps.sh .
RUN sh ./install-build-deps.sh

# ENV PATH=/usr/local/cargo/bin:$PATH
# RUN rustup --version && cargo --version && rustc --version

COPY ./build-influx.sh .
RUN sh ./build-influx.sh

COPY . .
EXPOSE 9999
CMD [ "sh", "./entrypoint.sh" ]