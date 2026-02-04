# Video Over UART

Sending low-res monochrome video from the PC to an embedded system over a serila port (UART).


## .NET Video Sender Application 

May need to install OpenCV. To do so, run from PowerShell:

`.\download_opencv_windows.ps1`


## Serial (UART) Protocol

The PC streams raw 1bpp frames over UART. There is no explicit end-of-frame marker, the receiver must know the frame size.

- Baud rate: using 460800 in the examples
- Frame start sequence: `0x69 0x01`
- Escape: if a data byte is `0x69`, it is sent as `0x69 0x69`
- Frame payload: `width * height / 8` bytes (e.g., 128 x 128 = 2048 bytes)
- Pixels order:
  - Iterate `y` in steps of 8 (page order), then `x` across the row
  - Each byte packs 8 vertical pixels at `(x, y..y+7)`
  - Bit 0 = `y+0`, bit 7 = `y+7`


# Embedded examples
There are 2 examples of receiving the video:
- Tiny RP2040 + 128 x 64 OLED (rp2040_Firmware folder)
- ESP32, Arduino + SOG128128A_T112 128 x 128 Transparent OLED (SOG128128A_Firmware folder)



