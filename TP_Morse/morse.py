import re
import time
import serial
import os, sys
from getkey import getkey


# Just for DEBUG
from pprint import pprint

USB_PORT_NUM        = 9600
MAX_DURATION        = 10  # express the duration that elpased from the last "1" to the actual "1"
LETTERS_AND_CODE    = {"A": "-_", "B": "_---", "C": "_-_-", "D": "_--", "E": "-"}
CODE_VALUE          = {"-": "1", "_": "2"}
CMD_NB_ARGUMENTS    = 3
LETTER_DELIMITER    = len(CODE_VALUE) + 2



def getIndexNextSignal(signalList, currentIndex, currentSignal):
    return [index for index, (signal, seconds) in enumerate(signalList[currentIndex: ]) if signal != currentSignal][0]


def getElapsedTime(signalList):
    
    lastSignalSeen  = signalList[0][0]
    timeElapsed     = signalList[0][1]
    timeElapsedList = []

    for index, (signal, seconds) in enumerate(signalList):
        if signal != lastSignalSeen:
            timeElapsedList.append((signal, signalList[index][1] - timeElapsed))
            timeElapsed  = seconds 
        
    return timeElapsedList


def getSignalsIndices(signalList):
    indices = [0]
    lastSignalSeen = -1
    for index, (signal, _) in enumerate(signalList):
        if lastSignalSeen == signal:
            continue
        else:
            lastSignalSeen = signal
            indices.append((signal, getIndexNextSignal(signalList, index, signal)))




def decode(signalList):
    timeElapsedList = getElapsedTime(signalList)
    output = ""

    for signal, timeElapsed in timeElapsedList:
        output += "-" if timeElapsed < MAX_DURATION else "_"

    return output


if __name__ == "__main__":

    if len(sys.argv) < CMD_NB_ARGUMENTS:
        print("\n\nUsage:\npython3 morse.py [USB_PORT_NAME] [PC | Arduino]")
        exit(1)

    USBPortName = sys.argv[1]
    ser         = serial.Serial(USBPortName, USB_PORT_NUM)
    action      = sys.argv[2]
    
    if action.lower() == "pc":
        keyPressed = ""
        while(keyPressed.lower() != "\x1b"):
            keyPressed = getkey().upper()
            print("The key pressed is : " + repr(keyPressed))
            if keyPressed in LETTERS_AND_CODE.keys():
                letterCode = LETTERS_AND_CODE[keyPressed]
                for char in letterCode:
                    charCode = CODE_VALUE[char]
                    print('{} ==> {}'.format(
                        char,
                        charCode
                    ))
                    
                    ser.write(bytes(charCode, "utf8"))
                    ser.write(bytes(str(LETTER_DELIMITER - 1), "utf8"))
            
            ser.write(bytes(str(LETTER_DELIMITER), 'utf8'))


                    # ser.write(0)
           
    else: 

        tStart = time.time()
        output = []
        maxTime = 10
        while((time.time() - tStart) < maxTime):
            value = [int(car) for car in ser.read().split() if car.isdigit()]
            value = value[0] if value else -1
            if value == 0 or value == 1:
                output.append((value, time.time() - tStart))
                # print("Value got {}".format(value))

        
                
        pprint(output)
        print(decode(output))