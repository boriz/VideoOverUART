cmake_minimum_required(VERSION 3.13)

include($ENV{PICO_SDK_PATH}/external/pico_sdk_import.cmake)

project(video C CXX ASM)

set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)

pico_sdk_init()

add_executable(video main.c ssd1306.c)

pico_enable_stdio_usb(video 1)
pico_enable_stdio_uart(video 1)
pico_add_extra_outputs(video)

target_link_libraries(video pico_stdlib hardware_i2c pico_multicore)
