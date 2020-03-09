/*

*/
#include <assert.h>
#include <stdlib.h>

#define BUTTON_PIN 5
#define BUZZER_PIN 6

#define LETTER_DELIMITER '4'
#define SHORT_TONE_VALUE '1'
#define LONG_TONE_VALUE '2'
#define SHORT_TONE_TIME 200
#define FREQUENCE 1000

#define PC_MODE '8'
#define ARDUINO_MODE '9'

#define MSG_STEP_SIZE 100


bool arduinoMode = true;

const char letters[]        = {'A', 'B', 'C', 'D', 'E', 'F', 'G',
                               'H', 'I', 'J', 'K', 'L', 'M', 'N',
                               'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                               'V', 'W', 'X', 'Y', 'Z',

                               '1', '2', '3', '4', '5',
                               '6', '7', '8', '9', '0',

                               ' '
                              };

int lettersCode[][6]        = {{49, 50}, {500, 49, 49, 49}, {50, 49, 50, 49}, {50, 49, 49}, {49}, {49, 49, 50, 49}, {50, 50, 49},
  {49, 49, 49, 49}, {49, 49}, {49, 50, 50, 50}, {50, 49, 50}, {49, 50, 49, 49}, {50, 50}, {50, 49},
  {50, 50, 50}, {49, 50, 50, 49}, {50, 50, 49, 50}, {49, 50, 49}, {49, 49, 49}, {50}, {49, 49, 50},
  {49, 49, 49, 50}, {49, 50, 50}, {50, 49, 49, 50}, {50, 49, 50, 50}, {50, 50, 49, 49},

  {49, 50, 50, 50, 50}, {49, 49, 50, 50, 50}, {49, 49, 49, 50, 50}, {49, 49, 49, 49, 50}, {49, 49, 49, 49, 49},
  {50, 49, 49, 49, 49}, {50, 50, 49, 49, 49}, {50, 50, 50, 49, 49}, {50, 50, 50, 50, 49}, {50, 50, 50, 50, 50},

  {49, 49, 49, 49, 49, 49}
};

static char *_message       = NULL;
static int _msgCurrentSize  = 0;
static int _msgMaxSize      = MSG_STEP_SIZE;


void setup() {

  Serial.begin(9600);
  pinMode(BUTTON_PIN, INPUT);
  pinMode(BUZZER_PIN, OUTPUT);
  _message = malloc(_msgMaxSize);
  assert(_message);
  
}


char decode(int values[10], int size) {

  for (int i = 0; i < size; ++i) {

  }

}

void printMessage(char letter) {

  if (_msgCurrentSize >= _msgMaxSize) {
    _msgMaxSize += MSG_STEP_SIZE;
    _message = realloc(_message, _msgMaxSize);
    assert(_message);
  }

  _message[_msgCurrentSize] = letter;
  Serial.println(_message);
}

void loop() {
  
	Serial.println(digitalRead(BUTTON_PIN));
	
	if (!Serial.available()) return;

	int values[10] = {0}, i = 0;


	for (int value = Serial.read(); value != LETTER_DELIMITER; value = Serial.read(), ++i) { // 52 the ascii code of the number 4 that is a delimiter of letters

		if (value == SHORT_TONE_VALUE) {
		  values[i] = value;
		  tone(BUZZER_PIN, FREQUENCE);
		  delay(SHORT_TONE_TIME);
		  noTone(BUZZER_PIN);
		  delay(100);
		}

		else if (value == LONG_TONE_VALUE) {
		  values[i] = value;
		  tone(BUZZER_PIN, FREQUENCE);
		  delay(SHORT_TONE_TIME * 2);
		  noTone(BUZZER_PIN);
		  delay(100);

		}

		else {
		  noTone(BUZZER_PIN);
		  delay(SHORT_TONE_TIME);
		}

	}

	delay(100);
  
}
