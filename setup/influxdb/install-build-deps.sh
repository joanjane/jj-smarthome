#!/bin/bash
set -e

# apt-get update && apt-get install git curl nano build-essential bzr protobuf-compiler libprotobuf-dev -y
echo "Installed global deps!"

cargo --version
rustc --version
rustup --version

##############        install rust      #############
# export RUSTUP_HOME=/usr/local/rustup
# export CARGO_HOME=/usr/local/cargo

# mkdir -p /usr/local/rustup
# mkdir -p /usr/local/cargo

# curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | bash -s -- -y --profile minimal

# chmod -R a+w $RUSTUP_HOME $CARGO_HOME

# cat <<EOF > $HOME/.cargo/config
# [target.armv7-unknown-linux-gnueabihf]
# linker = "arm-linux-gnueabihf-gcc-5"
# EOF

# export PATH=$HOME/.cargo/bin:$PATH
# source $HOME/.cargo/env
# cat $HOME/.cargo/config
# echo "Installed rust!"

# rustup --version
# # $HOME/.cargo/bin/cargo --version
# cargo --version
# rustc --version

##############         install go       #############
# https://dl.google.com/go/go1.13.15.linux-arm64.tar.gz | tar -C /usr/local -xzf
# export PATH=$PATH:/usr/local/go/bin
# echo "Installed go!"

##############      install node+yarn   #############
curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | apt-key add -
echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list

curl -sL https://deb.nodesource.com/setup_lts.x | bash -

apt update && apt-get install -y nodejs yarn
echo "Installed node and yarn!"
