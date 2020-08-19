#!/bin/bash

apt-get update && apt-get install git curl nano build-essential bzr protobuf-compiler libprotobuf-dev -y

curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | apt-key add -
echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list

curl -sL https://deb.nodesource.com/setup_lts.x | bash -
apt update && apt-get install -y nodejs yarn

apt remove cmdtest

mkdir -p /usr/local/rustup
mkdir -p /usr/local/cargo

curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | bash -s -- -y --profile minimal

chmod -R a+w $RUSTUP_HOME $CARGO_HOME

# rustup --version
# cargo --version
# rustc --version
