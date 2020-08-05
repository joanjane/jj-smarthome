docker build -f "./dev.Dockerfile" -t dev-jj-smarthome-sensehat .
docker run --rm -it -v "$(pwd):/src" dev-jj-smarthome-sensehat /bin/sh
