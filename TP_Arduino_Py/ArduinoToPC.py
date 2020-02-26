
import serial
import keyboard
import os

USB_PORT_NAME   = "/dev/ttyACM0"
USB_PORT_NUM    = 9600
LED_PIN         = 6


if __name__ == "__main__":

    ser = serial.Serial(USB_PORT_NAME, USB_PORT_NUM)

    while(not keyboard.is_pressed("esc")):
        value = [int(car) for car in ser.read().split() if car.isdigit()]
        value = value[0] if value else []
        # print(value)
        if value == 1:
            # os.system("beep -f 555 -l 460")
            print("BEEP")
            beep = lambda x: os.system("echo -n '\a';sleep 0.2;" * x)
            beep(1)