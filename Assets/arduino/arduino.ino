const int troll_Pin = 23;
const int zombieQuick_Pin = 24;
const int zombieNormal_Pin = 27;
const int SW_pin = 50; // digital pin connected to switch output
const int X_pin = A1; // analog pin connected to X output
const int Y_pin = A0; // analog pin connected to Y output
const int bossMelee_pin = 8;
const int bossMinion_pin = 9;
const int bossMeteor_pin = 12;

const int debounceDelay = 150;

int troll_State = 0;
int zombieQuick_State = 0;
int zombieNormal_State = 0;
int spawningLight_State = 0;
int xAxis = 0;
int yAxis = 0;
int bossMelee_State = 0;
int bossMinion_State = 0;
int bossMeteor_State = 0;

int xDefault_RangeMin = 400;
int xDefault_RangeMax = 600;
int yDefault_RangeMin = 400;
int yDefault_RangeMax = 600;

void setup() {
  Serial.begin(9600);
  pinMode(troll_Pin,INPUT_PULLUP);
  pinMode(zombieQuick_Pin,INPUT_PULLUP);
  pinMode(zombieNormal_Pin,INPUT_PULLUP);
  pinMode(bossMelee_pin,INPUT_PULLUP);
  pinMode(bossMinion_pin,INPUT_PULLUP);
  pinMode(bossMeteor_pin,INPUT_PULLUP);
  pinMode(SW_pin, INPUT);
  digitalWrite(SW_pin, HIGH);
}

void loop() {
  // read the state of the pushbutton value:
  troll_State = digitalRead(troll_Pin);
  zombieQuick_State = digitalRead(zombieQuick_Pin);
  zombieNormal_State = digitalRead(zombieNormal_Pin);
  spawningLight_State = digitalRead(SW_pin);
  xAxis = analogRead(X_pin);
  yAxis = analogRead(Y_pin);
  bossMelee_State = digitalRead(bossMelee_pin);
  bossMinion_State = digitalRead(bossMinion_pin);
  bossMeteor_State = digitalRead(bossMeteor_pin);

  if (troll_State == LOW) {
    Serial.write("3");
    Serial.flush();
  }

  if(zombieQuick_State == LOW){
    Serial.write("2");
    Serial.flush();
  }
  if(zombieNormal_State == LOW){
    Serial.write("1");
    Serial.flush();
  }
  if(spawningLight_State == LOW){
    Serial.write("9");
    spawningLight_State = HIGH;
  }  
  if(xAxis <= xDefault_RangeMin)
    Serial.write("8");
  else if(xAxis >= xDefault_RangeMax)
    Serial.write("7");

  if(bossMelee_State == LOW)
  {
    Serial.write("6");
  }
  if(bossMinion_State == LOW)
  {
    Serial.write("5");
  }
  if(bossMeteor_State == LOW)
  {
    Serial.write("4");
  }

  //Serial.println(digitalRead(40));

  /*Serial.print("Switch:  ");
  Serial.print(digitalRead(SW_pin));
  Serial.print("\n");
  Serial.print("X-axis: ");
  Serial.print(analogRead(X_pin));
  Serial.print("\n");
  Serial.print("Y-axis: ");
  Serial.println(analogRead(Y_pin));
  Serial.print("\n\n");
  delay(2000);*/
  
  delay(debounceDelay);
}
