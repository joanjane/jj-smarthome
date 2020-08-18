#!/bin/bash

apt-get update && apt-get install git curl nano build-essential bzr protobuf-compiler libprotobuf-dev yarnpkg -y

mkdir -p /usr/local/rustup
mkdir -p /usr/local/cargo

curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs -o rust-init
chmod +x rust-init

./rustup-init -y --no-modify-path --profile minimal
rm rustup-init; \
chmod -R a+w $RUSTUP_HOME $CARGO_HOME

rustup --version
cargo --version
rustc --version