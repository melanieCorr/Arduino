
import re
import time
import serial


# Just for DEBUG
from pprint import pprint

USB_PORT_NAME   = "/dev/ttyACM0"
USB_PORT_NUM    = 9600
MAX_DURATION    = 10  # express the duration that elpased from the last "1" to the actual "1"
LETTERS         = ["A", "B", "C", "D", "E"]
LETTERS_CODE    = ["-_", "_---", "_-_-", "_--","-"]



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

    ser = serial.Serial(USB_PORT_NAME, USB_PORT_NUM)
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