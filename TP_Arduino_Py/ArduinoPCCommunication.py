
import serial
import os, sys
import keyboard

USB_PORT_NUM        = 9600
LED_PIN             = 6
CMD_NB_ARGUMENTS    = 2

if __name__ == "__main__":

    if len(sys.argv) < CMD_NB_ARGUMENTS:
        print("\n\nUsage:\npython3 ArduinoPCCommunication.py [USB_PORT_NAME]")
        exit(1)

    USBPortName = sys.argv[1]
    ser         = serial.Serial(USBPortName, USB_PORT_NUM)

    while(not keyboard.is_pressed("esc")):
        
        if keyboard.is_pressed("a"):
            ser.write(1)
        else:
            ser.write(0)
        
        # value = [int(car) for car in ser.read().split() if car.isdigit()]
        # value = value[0] if value else []

        # if value == 1:
        #     print("BEEP")
        #     beep = lambda x: os.system("echo -n '\a';sleep 0.2;" * x)
        #     beep(3)