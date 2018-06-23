#include <SoftwareSerial.h> //Software Serial Port
#define RxD 14
#define TxD 15

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

SoftwareSerial blueToothSerial(RxD,TxD);

void setup() {
  Serial.begin(9600);
  blueToothSerial.begin(10000);
  pinMode(RxD, INPUT);
  pinMode(TxD, OUTPUT);
  pinMode(troll_Pin,INPUT_PULLUP);
  pinMode(zombieQuick_Pin,INPUT_PULLUP);
  pinMode(zombieNormal_Pin,INPUT_PULLUP);
  pinMode(bossMelee_pin,INPUT_PULLUP);
  pinMode(bossMinion_pin,INPUT_PULLUP);
  pinMode(bossMeteor_pin,INPUT_PULLUP);
  pinMode(SW_pin, INPUT);
  digitalWrite(SW_pin, HIGH);
 /* blueToothSerial.print("\r\n+STWMOD=0\r\n"); 
  blueToothSerial.print("\r\n+STPIN=0000\r\n");//Set SLAVE pincode"0000"
  blueToothSerial.print("\r\n+STOAUT=1\r\n"); //Permit Paired device to connect me
  blueToothSerial.print("\r\n+INQ=1\r\n"); */
  blueToothSerial.flush();
}

String str10 = "Nivel 1\n";
String str11 = "Cooldown:4s\n";
String str12 = "Z Normal: v=100,s=2\n";
String str13 = "Q Zombie: v=50,s=4\n";
String str14 = "Troll: v=150,s=1\n";
String str15 = "Boss: v=300,s=200";

String str20 = "Nivel 2\n";
String str21 = "Cooldown Time:2s\n";
String str22 = "Zombie Normal: v=100,s=2\n";
String str23 = "Quick Zombie: v=50,s=4\n";
String str24 = "Troll: v=150,s=1\n";
String str25 = "Boss: v=300,s=200";

String str30 = "Nivel 3\n";
String str31 = "Cooldown Time:1s\n";
String str32 = "Zombie Normal: v=100,s=2\n";
String str33 = "Quick Zombie: v=50,s=4\n";
String str34 = "Troll: v=150,s=1\n";
String str35 = "Boss: v=300,s=200";

int canWrite = 0;
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

  char recvChar;

  if(Serial.available()){ //check if there's any data sent from the remote BT shield
   recvChar = Serial.read();
   Serial.flush();
   canWrite = recvChar - '0';
   }

   if(canWrite == 1)
   {
       blueToothSerial.print(str10);
       delay(1000);
       blueToothSerial.print(str11);
       delay(1000);
       blueToothSerial.print(str12);
       delay(1000);
       blueToothSerial.print(str13);
       delay(1000);
       blueToothSerial.print(str14);
       delay(1000);
       blueToothSerial.print(str15);

      canWrite = 0;
   }
   else if(canWrite == 2)
   {
       blueToothSerial.print(str20);
       delay(1000);
       blueToothSerial.print(str21);
       delay(1000);
       blueToothSerial.print(str22);
       delay(1000);
       blueToothSerial.print(str23);
       delay(1000);
       blueToothSerial.print(str24);
       delay(1000);
       blueToothSerial.print(str25);

      canWrite = 0;
   }
   else if(canWrite >= 3)
   {
       blueToothSerial.print(str30);
       delay(1000);
       blueToothSerial.print(str31);
       delay(1000);
       blueToothSerial.print(str32);
       delay(1000);
       blueToothSerial.print(str33);
       delay(1000);
       blueToothSerial.print(str34);
       delay(1000);
       blueToothSerial.print(str35);

      canWrite = 0;
   }

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

  delay(debounceDelay);
}
