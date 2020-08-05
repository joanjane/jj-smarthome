# Introduction
This project uses sensehat to provide environmental info like temperature, humidity and pressure.

## Configuration
Create a .env file based on .env.sample.
```bash
# MODE to use real sense hat or the simulated interface
MODE=[production|simulator]

# Simulator settings 
# (see pi-sense-hat-remote-simulator repo to launch web simulator and server for development)
SERVER_URI=ws://localhost:8080
DEVICE=pi-sense-hat-library-sample # identifier of the simulator must match on the web simulator
```