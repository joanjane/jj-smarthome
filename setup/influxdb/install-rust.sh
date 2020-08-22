#!/bin/bash
set -e

# update system
apt-get update && \
  apt-get install -y curl git gcc xz-utils sudo pkg-config unzip

# config and set variables
export PI_TOOLS_GIT_REF=master
export RUST_VERSION=stable
export URL_GIT_PI_TOOLS=https://github.com/raspberrypi/tools.git
export TOOLCHAIN_64=$HOME/pi-tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian-x64/bin
export TOOLCHAIN_32=$HOME/pi-tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian/bin

# clone rust setup scripts
git clone https://github.com/Ragnaroek/rust-on-raspberry-docker.git

# install rustup with raspberry target
sh ./rust-on-raspberry-docker/build/download-rust.sh

cp ./rust-on-raspberry-docker/bin/gcc-sysroot $HOME/pi-tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian-x64/bin/gcc-sysroot
cp ./rust-on-raspberry-docker/bin/gcc-sysroot $HOME/pi-tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian/bin/gcc-sysroot

# configure cargo
cp ./rust-on-raspberry-docker/conf/cargo-config $HOME/.cargo/config

cp -r ./rust-on-raspberry-docker/bin $HOME/bin

export PATH=$HOME/bin:$PATH
source $HOME/.cargo/env
echo $PATH