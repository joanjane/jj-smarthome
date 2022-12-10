# docker build -t jj-smarthome-sensehat-dev -f dev.Dockerfile .
# docker run --rm -it -v ${PWD}:/src --env-file .env.docker jj-smarthome-sensehat-dev
FROM node:8-buster
VOLUME [ "/src" ]
WORKDIR /src

RUN apt-get update && apt-get install build-essential \
    python \
    make \
    g++ \
    nano \
    -y

ENTRYPOINT /bin/bash