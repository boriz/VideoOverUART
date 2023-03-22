#!/bin/bash

docker run --rm -it -u $(id -u):$(id -g) --mount type=bind,source=${PWD},target=/home/dev -w /home/dev lukstep/raspberry-pi-pico-sdk ./build.sh
