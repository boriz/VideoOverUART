#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <stdlib.h>
#include "pico/binary_info.h"
#include "pico/multicore.h"
#include "pico/util/queue.h"
#include "hardware/gpio.h"
#include "hardware/i2c.h"

#include "ssd1306.h"


//#define PICO_DEFAULT_UART_BAUD_RATE 921600
//#define CFG_TUD_CDC_EP_BUFSIZE 1024 
//#define CFG_TUD_CDC_RX_BUFSIZE 1024
//#define CFG_TUD_CDC_TX_BUFSIZE 1024
#define UART_ID uart0
#define I2C_ID i2c1

void UartLoop();

queue_t buffer_queue;
int buf_size;


int main ()
{
    bi_decl(bi_program_description("Video Over Serial"));
    stdio_init_all();

    // Configure I2C
    i2c_init(I2C_ID, 400000);
    gpio_set_function(2, GPIO_FUNC_I2C);    // I2C1 SDA
    gpio_pull_up(2);
    gpio_set_function(3, GPIO_FUNC_I2C);    // I2C1 SCL    
    gpio_pull_up(3);

    // Configure display
    ssd1306_t disp;
    disp.external_vcc = false;
    ssd1306_init(&disp, 128, 64, 0x3C, I2C_ID);
    ssd1306_clear(&disp);

    // Configure queue
    buf_size = disp.bufsize;
    queue_init(&buffer_queue, buf_size, 5);

    // Start UART loop
    multicore_launch_core1(UartLoop);

    // Start core 0 main loop
    absolute_time_t time_start = get_absolute_time(); 
    int fps = 0;
    char fps_str[20] = "";
    uint8_t* p_buff = malloc(buf_size);
    memset(p_buff, 0, buf_size);
    while (true)
    {
        if (queue_try_remove(&buffer_queue, p_buff))
        {
            // Something is in the queue. Update buffer
            memcpy(disp.buffer, p_buff, buf_size);
            fps++;
        }
        
        if ( absolute_time_diff_us(get_absolute_time(), time_start) <= 0 )
        {
            // Time to print FPS counter
            // Restart timer
            time_start = make_timeout_time_ms(1000);

            // Update FPS                        
            sprintf(fps_str, "FPS:%02d", fps);
            printf("FPS: %d \n", fps);
            fps = 0;    
        }

        // Update OLED        
        ssd1306_draw_square_black(&disp, 88, 0, 38, 10);
        ssd1306_draw_string(&disp, 90, 0, 1, fps_str);
        ssd1306_show(&disp);                    
    }
}


void UartLoop()
{
    // Configure UART
    uart_init(UART_ID, 460800);
    gpio_set_function(0, GPIO_FUNC_UART);   // UART0 TX
    gpio_set_function(1, GPIO_FUNC_UART);   // UART0 RX
    uart_set_hw_flow(UART_ID, false, false);
    uart_set_format(UART_ID, 8, 1, UART_PARITY_NONE);
    uart_set_fifo_enabled(UART_ID, true);

    // Other variables
    bool special_char = false;
    uint8_t* p_buff = malloc(buf_size);
    int draw_index = 0;
    
    while (true)
    {
        // Try to read from serial port
        uint8_t new_serial_byte;

        // Exit if on more charcters
        if (!uart_is_readable(UART_ID))
        {
            continue;
        }

        // Got new character
        new_serial_byte = uart_getc(UART_ID);
        //printf("C: 0x%02X \n", new_serial_byte);

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
                //printf("Frame start. Prev frame index: %d \n", draw_index);
                draw_index = 0;                
                if ( !queue_try_add(&buffer_queue, p_buff) )
                {
                    printf("Fail adding to queue \n");
                }

                memset(p_buff, 0, buf_size);
                continue;
            }                
        }

        // Get a normal data byte
        // Update buffer
        p_buff[draw_index] = new_serial_byte;            
        //printf("Draw byte: %02X, index: %d \n", new_serial_byte, draw_index);
        draw_index++;
    }
}



