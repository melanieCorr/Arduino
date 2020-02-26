/*

*/

#define BUTTON_PIN 6

void setup() {
    pinMode(BUTTON_PIN, INPUT);
    Serial.begin(9600);
}
void loop() {
  int value = digitalRead(BUTTON_PIN);
  
  
  Serial.println(value);

}
