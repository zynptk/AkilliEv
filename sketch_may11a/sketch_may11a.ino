#include <dht11.h> // dht11 kütüphanesini ekliyoruz.
#define DHT11PIN 2 // DHT11PIN olarak Dijital 2"yi belirliyoruz.

dht11 DHT11;
#define trigPin 11
#define echoPin 12

const int gasPin = A0; // Arduino nun A0 pinine sensörün analog çıkışı bağlanacak
int veri = 0;   
int val;
int tempPin = 1;
const int prob = A2;
int olcum_sonucu; 

void setup() { 
  Serial.begin(9600); 
  pinMode(gasPin, OUTPUT); 
  pinMode(trigPin, OUTPUT);
pinMode(echoPin, INPUT);

}
void loop() {
//
//val = analogRead(tempPin);
//float mv = ( val/1024.0)*5000; 
//float cel = mv/10;
//float farh = (cel*9)/5 + 32;
////
//////Serial.print("SICAKLIK = ");
//Serial.print(cel);
////Serial.print(",");
//////Serial.print("*C");
//////Serial.println();
//////delay(1000);
  int chk = DHT11.read(DHT11PIN);
 Serial.print((float)DHT11.temperature);
Serial.print(",");

//Serial.print("TOPRAK NEMİ = ");
olcum_sonucu = analogRead(prob); 
Serial.print(olcum_sonucu);
Serial.print(",");
//Serial.print("\n");
long sure, mesafe;
digitalWrite(trigPin, LOW);
delayMicroseconds(2);
digitalWrite(trigPin, HIGH);
delayMicroseconds(10);
digitalWrite(trigPin, LOW);
sure = pulseIn(echoPin, HIGH);
mesafe = (sure/2) / 29.1;

    //Serial.print("Uzaklık: ");
   Serial.print(mesafe);
   Serial.print(",");
   //Serial.println(" cm");
   


//delay(500); 
  veri = analogRead(gasPin);        
 
  //Serial.print("GAZ = " );                       
  Serial.print(veri);  
  Serial.print(","); 
            
unsigned int isiksiddeti = analogRead(A3);
//Serial.print("Işık Şiddeti: ");
Serial.print(isiksiddeti);

delay(5000);  
Serial.print(",");
//Serial.println("-----------------------");
}
