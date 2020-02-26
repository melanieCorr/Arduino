
import serial
import keyboard


USB_PORT_NAME   = "/dev/ttyACM0"
USB_PORT_NUM    = 9600
LED_PIN         = 6


if __name__ == "__main__":

    ser = serial.Serial(USB_PORT_NAME, USB_PORT_NUM)
    #pinMode(LED_PIN, 1)
    while(not keyboard.is_pressed("esc")):
        if keyboard.is_pressed("a"):
            ser.write(1)
        else:
            ser.write(0)
