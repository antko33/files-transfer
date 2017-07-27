C:\Users\antko\OneDrive\Document <Time.h>
#include <TimeLib.h>

#include <TM1637.h>
#include <Wire.h>

#define CLK 6
#define DIO 7
#define DS3231_I2C_ADDRESS 0x68

TM1637 tm1637(CLK, DIO);
int8_t disp[4];
int point = 1;

int64_t ms_pts = 0;
int64_t ms_rtc = 0;

void setup() {
  // put your setup code here, to run once:
  tm1637.init();
  tm1637.set(1);
  tm1637.point(POINT_ON);

  setSyncProvider(RTC.get);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(timeStatus() == timeSet)
  {
    if(millis() - ms_rtc >= 1000)
    {
      ms_rtc = millis();
      ClockDisplay();
    }
  }
  else
  {
    delay(4000);
  }
  
  if(millis() - ms_pts >= 500)
  {
    ms_pts = millis();
    if (point == 1)
    {
      point = 0;
      tm1637.point(POINT_OFF);
    }
    else
    {
      point = 1;
      tm1637.point(POINT_ON);
    }
  }

  tm1637.display(disp);
}

void ClockDisplay()
{
  disp[0] = hour() / 10;
  disp[1] = hour() % 10;
  disp[2] = minute() / 10;
  disp[3] = minute() % 10;
}
               