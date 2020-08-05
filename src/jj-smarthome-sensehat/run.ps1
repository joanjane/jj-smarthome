docker build -t jj-smarthome-sensehat .
docker run --rm -it --env-file="./.env" jj-smarthome-sensehat
