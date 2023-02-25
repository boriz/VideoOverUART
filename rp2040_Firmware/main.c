#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <stdlib.h>
#include "pico/binary_info.h"
#include "pico/multicore.h"
#include "hardware/gpio.h"
#include "hardware/i2c.h"

#include "ssd1306.h"


//#define PICO_DEFAULT_UART_BAUD_RATE 921600
#define CFG_TUD_CDC_EP_BUFSIZE 1024 
#define CFG_TUD_CDC_RX_BUFSIZE 1024
#define CFG_TUD_CDC_TX_BUFSIZE 1024


void MainLoop();


int main ()
{
    bi_decl(bi_program_description("Video Over Serial"));
    stdio_init_all();

    multicore_launch_core1(MainLoop);

    while (true)
    {
        sleep_ms(1);        
    }
}


void MainLoop()
{
    // Configure I2C
    i2c_init(i2c1, 400000);
    gpio_set_function(2, GPIO_FUNC_I2C);    // I2C1 SDA
    gpio_set_function(3, GPIO_FUNC_I2C);    // I2C1 SCL
    gpio_pull_up(2);
    gpio_pull_up(3);

    // Configure display
    ssd1306_t disp;
    disp.external_vcc = false;
    ssd1306_init(&disp, 128, 64, 0x3C, i2c1);
    ssd1306_clear(&disp);

    // Other variables
    bool special_char = false;
    uint8_t* p_buff = malloc(disp.bufsize);
    int draw_index = 0;
    absolute_time_t time_start = get_absolute_time(); 
    int fps = 0;
    char fps_str[20];

    while (true)
    {
        // Try to read from serial port
        int new_serial_int;
        uint8_t new_serial_byte;
        while(true)
        {
            new_serial_int = getchar_timeout_us(0);
            if (new_serial_int != PICO_ERROR_TIMEOUT)
            {   // Got new character
                new_serial_byte = (uint8_t) (new_serial_int & 0xff);
                //printf("C: 0x%02X \n", new_serial_byte);
            }
            else
            {
                // No more serial characters, exit the loop
                break;
            }

            // Receive special char
            if (new_serial_byte == 0x69 && !special_char)
            {
                // Special character, set the flag
                //printf("Special char \n");
                special_char = true;
                continue;
            }

            // Are we expecting a special command?
            if (special_char)
            {
                special_char = false;
                if (new_serial_byte == 0x01)
                {
                    // Start of the frame
                    //printf("Frame start \n");
                    draw_index = 0;
                    memcpy(disp.buffer, p_buff, disp.bufsize);
                    memset(p_buff, 0, disp.bufsize);
                    continue;
                }                
            }

            // Get a normal data byte
            // Update buffer
            p_buff[draw_index] = new_serial_byte;            
            //printf("Draw byte: %02X, index: %d \n", new_serial_byte, draw_index);
            draw_index++;
        }

        if ( absolute_time_diff_us(get_absolute_time(), time_start) <= 0 )
        {
            // Time to print FPS counter
            // Restart timer
            time_start = make_timeout_time_ms(1000);

            // Update FPS                        
            //sprintf(fps_str, "FPS:%02d", fps);
            printf("FPS: %d \n", fps);
            fps = 0;    
        }

        // Updated OLED        
        //ssd1306_draw_string(&disp, 90, 0, 1, fps_str);
        ssd1306_show(&disp);            

        fps++;
        sleep_ms(1);
    }
}



