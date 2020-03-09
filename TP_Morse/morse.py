import re
import time
import serial
import os, sys
from getkey import getkey


# Just for DEBUG
from pprint import pprint

CMD_NB_ARGUMENTS    = 3
USB_PORT_NUM        = 9600
MAX_DURATION        = 10  # express the duration that elpased from the last "1" to the actual "1"
LETTERS_AND_CODE    = { "A": "-_", "B": "_---", "C": "_-_-", "D": "_--", "E": "-",
                        "F": "--_-", "G": "__-", "H": "----", "I": "--", "J": "-___",
                        "K": "_-_", "L": "-_--", "M": "__", "N": "_-", "O": "___",
                        "P": "-__-", "Q": "__-_", "R": "-_-", "S": "---", "T": "_",
                        "U": "--_", "V": "---_", "W": "-__", "X": "_--_", "Y": "_-__",
                        "Z": "__--",

                        "1": "-____", "2": "--___", "3": "---__", "4": "----_", "5": "-----",
                        "6": "_----", "7": "__---", "8": "___--", "9": "____-", "0": "_____" 
                    }

CODE_VALUE          = {"-": "1", "_": "2"}
LETTER_DELIMITER    = len(CODE_VALUE) + 2

SHORT_TONE_DURATION = 0.17
BET_TONES_DURATION  = 0.6
BET_WORDS_DURATION  = 1.6

PC_MODE             = '8'
ARDUINO_MODE        = '9'



def getElapsedTime(signalList):
    
    lastSignalSeen  = signalList[0][0]
    timeElapsed     = signalList[0][1]
    timeElapsedList = []
    length          = len(signalList) - 1

    for index, (signal, seconds) in enumerate(signalList):
        if index == length:
            timeElapsedList.append((lastSignalSeen, seconds - timeElapsed))

        if signal != lastSignalSeen:
            timeElapsedList.append((lastSignalSeen, signalList[index - 1][1] - timeElapsed))
            timeElapsed     = seconds 
            lastSignalSeen  = signal
        
    return timeElapsedList


def letterFinished(timeElapsedList, index, signal=1):

    # check if we are in the bounds of the list with (index -2) otherwise we'll get errors
    # (index - 2) because each element of the list is different from its previous (ie: [(0, 4.56), (1, 0.67), (0, 0.62), (1, 0.08)]) 
    if signal == 1:
        return (timeElapsedList[index - 1][1] >= BET_TONES_DURATION and timeElapsedList[index - 1][1] < BET_WORDS_DURATION) if (index - 2) >= 0 else False
    else:
        return (timeElapsedList[index][1] >= BET_TONES_DURATION and timeElapsedList[index][1] < BET_WORDS_DURATION)


def wordFinished(timeElapsedList, index, signal=1):

    # check if we are in the bounds of the list with (index -2) otherwise we'll get errors
    # (index - 2) because each element of the list is different from its previous (ie: [(0, 4.56), (1, 0.67), (0, 0.62), (1, 0.08)]) 
    if signal == 1:
        return  timeElapsedList[index - 1][1] >= BET_WORDS_DURATION if (index - 2) >= 0 else False
    else:
        return  timeElapsedList[index][1] >= BET_WORDS_DURATION 



def decodeSignal(signalList):
    
    timeElapsedList = getElapsedTime(signalList)
    output = ""

    for index, (signal, timeElapsed) in enumerate(timeElapsedList):
        if index == 0:
            continue

        if wordFinished(timeElapsedList, index, 0):
            output += "w"

        elif letterFinished(timeElapsedList, index, 0):
            output += "l"
        
        if signal == 1:
           
            output += "-" if timeElapsed < SHORT_TONE_DURATION else "_"

    return output


def getKeyByValue(targetedValue):

    for key, value in LETTERS_AND_CODE.items():
        if value == targetedValue:
            return key
    return None



def interpretDecodedSignal(decodedSignal):

    letterCode, output = '', ''
    delimiters = ['l', 'w']

    for char in decodedSignal:
        if char in delimiters:
            letter = getKeyByValue(letterCode)
            output += letter if letter else ''
            letterCode = ''
            if char == 'w':
                output += ' '
        else:
            letterCode += char

    return output



def getNbCharsInString(string, charsList):
    
    nb = 0
    for char in string:
        if char in charsList:
            nb += 1

    return nb



if __name__ == "__main__":

    if len(sys.argv) < CMD_NB_ARGUMENTS:
        print("\n\nUsage:\npython3 morse.py [USB_PORT_NAME] [PC | Arduino]")
        exit(1)

    USBPortName = sys.argv[1]
    ser         = serial.Serial(USBPortName, USB_PORT_NUM)
    action      = sys.argv[2].lower()
    keyPressed  = ""
    
    if action == "pc":

        ser.write(bytes(ARDUINO_MODE, "utf8"))

        while keyPressed.lower() != "\x1b":
            keyPressed = getkey().upper()
            print("The key pressed is : " + repr(keyPressed))
            if keyPressed in LETTERS_AND_CODE.keys():
                letterCode = LETTERS_AND_CODE[keyPressed]
                for char in letterCode:
                    charCode = CODE_VALUE[char]
                    print('\t{} ==> {}'.format(
                        char,
                        charCode
                    ))
                    
                    ser.write(bytes(charCode, "utf8"))
                    ser.write(bytes(str(LETTER_DELIMITER - 1), "utf8"))
            
            ser.write(bytes(str(LETTER_DELIMITER), 'utf8'))

           
    else: 

        ser.write(bytes(PC_MODE, "utf8"))

        tStart      = time.time()
        output      = [(0, 0.0)]
        maxTime     = 15
        prevMsgSize = 0
        curMsgSize  = 0

        while True :   

            value = [int(car) for car in ser.read().split() if car.isdigit()]
            value = value[0] if value else -1
            if value == 0 or value == 1:
                output.append((value, time.time() - tStart))


            decodedSignal   = decodeSignal(output)
            message         = interpretDecodedSignal(decodedSignal)
            curMsgSize      = len(message)

            if curMsgSize != prevMsgSize:
                prevMsgSize = curMsgSize
                print("\033c", end="")
                print("Message: {}".format(message))
               

































        # currentSize = len(decodedSignal)
        # if currentSize != previousSize:     
        #     # print("decodeSignal: {}".format(decodedSignal))

        #     previousSize = currentSize
        #     curNbLetterWordDelimiter = getNbCharsInString(decodedSignal, ['l', 'w'])    
            
        #     if curNbLetterWordDelimiter != prevNbLetterWordDelimiter:
        #         prevNbLetterWordDelimiter = curNbLetterWordDelimiter

        #         message = interpretDecodedSignal(decodedSignal)
        #         print("decodedSignal: {} ==> {}".format(decodedSignal, message))
                
        # pprint(output)
        # print("********************Decode**********************")
        # print(decode(output))