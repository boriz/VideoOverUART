# Video Over UART

Sending video from the PC to rp2040 over UART and showing it on a small OLED connected to rp2040.



## .NET Video Sender Application 





## RP2040 Firmware



`docker run --rm -it --mount type=bind,source=${PWD},target=/home/dev -w /home/dev lukstep/raspberry-pi-pico-sdk sh`



`mkdir build`
`cd build`
`cmake ..` 



`make -j4`
