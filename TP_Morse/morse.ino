/*

*/

#define BUTTON_PIN 5
#define BUZZER_PIN 6

#define LETTER_DELIMITER 52  //'4' - '0'
#define SHORT_TONE_VALUE 49  //'1' - '0' 
#define LONG_TONE_VALUE 50   //'2' - '0' 
#define SHORT_TONE_TIME 200
#define FREQUENCE 1000

void setup() {
  pinMode(BUTTON_PIN, INPUT);
  pinMode(BUZZER_PIN, OUTPUT);
  Serial.begin(9600);
}


void loop() {
  
  if (!Serial.available()) return;

  for (int value = Serial.read(); value != LETTER_DELIMITER; value = Serial.read()) { // 52 the ascii code of the number 4 that is a delimiter of letters
    Serial.print(String("***Value: ") + value + "\n");

    if (value == SHORT_TONE_VALUE) {
      tone(BUZZER_PIN, FREQUENCE); 
      delay(SHORT_TONE_TIME);
      //Serial.println("==> SHORT TONE");
      noTone(BUZZER_PIN);
      delay(100);
    }

    else if (value == LONG_TONE_VALUE) {
      tone(BUZZER_PIN, FREQUENCE); 
      delay(SHORT_TONE_TIME * 2);
      //Serial.println("==> LONG TONE");
      noTone(BUZZER_PIN);
      delay(100);

    }

    else {
      noTone(BUZZER_PIN);
      delay(SHORT_TONE_TIME);
      //Serial.println("==> NO TONE");
    }

  }

  Serial.println("Another letter\n\n");
  delay(100);

}