# Tiny RP2040 + 128 x 64 OLED

This examples uses a tiny OLED connecte to a rp2040 over I2C. 


## Hardware

* OLED: [ER-OLEDM0.96-1W-I2C](https://www.buydisplay.com/i2c-white-0-96-inch-oled-display-module-128x64-arduino-raspberry-pi): 
* MCU dev board: Tiny RP2040.

Pinout:
* Tiny Pin 0, UART0-TX = connect to USB UART adapter RX pin
* Tiny Pin 1, UART0-RX = connect to USB UART adapter TX pin
* Tiny Pin 2, I2C1-SDA = connect to OLED SDA pin
* Tiny Pin 3, I2C1-SCL = connect to OLED SCL pin

Obviously connect GND pins to USB/UART and OLED and connect OLED VCC to Tiny 3.3V pin.


## RP2040 Firmware
Compiling RP2040 firmware in a docker:

`docker run --rm -it --mount type=bind,source=${PWD},target=/home/dev -w /home/dev lukstep/raspberry-pi-pico-sdk sh`



`mkdir build`
`cd build`
`cmake ..` 



`make -j4`

