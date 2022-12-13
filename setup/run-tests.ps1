docker-compose `
-f ./docker-compose.testing.yml `
-f ./docker-compose.fakes.yml `
-f ./docker-compose.mqtt.yml `
-f ./docker-compose.db-v1.yml `
--env-file .development.env `
up --build `
--exit-code-from=jj-smarthome-webapi-tests `
jj-smarthome-webapi-tests mosquitto mailhog influxdb_v1

if($?) {
	"Tests passed!"
} else {
    "Tests failed"
    docker logs $(docker ps -aqf "name=jj-smarthome-webapi-tests")
}