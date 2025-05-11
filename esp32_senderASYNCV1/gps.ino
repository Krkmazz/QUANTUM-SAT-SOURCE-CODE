#include <TimeLib.h>
#include <HardwareSerial.h>
#include <TinyGPS++.h>


TinyGPSPlus gps;
// UART2 (GPS için)
HardwareSerial gpsSerial(2);
#define GPS_TX 13  // DİKKAT GPS TX -- >    ESP RX PİN OLUR
#define GPS_RX -1

void initGPS() {
  gpsSerial.begin(9600, SERIAL_8N1, GPS_TX, GPS_RX);
}


struct GPS_STRUCT {
  int32_t longitude = 0;
  int32_t latitude = 0;
  int16_t altitude = 0;
};

GPS_STRUCT gps_array;

void getGPSData() {
  while (gpsSerial.available() > 0) {
    gps.encode(gpsSerial.read());
    if (gps.location.isUpdated()) {
      gps_array.latitude = gps.location.lat();
      gps_array.longitude = gps.location.lng();
      gps_array.altitude = gps.altitude.meters();
    }
  }
}

int32_t getLongitudeGPS() {
  return gps_array.longitude;
}

int32_t getLatitudeGPS() {
  return gps_array.latitude;
}

int16_t getAltitudeGPS() {
  return gps_array.altitude;
}

// int32_t getDate_Time() {
//   int year = gps.date.year();
//   int month = gps.date.month();
//   int day = gps.date.day();
//   int hour = gps.time.hour() + 3;  //+3 dilimi
//   int minute = gps.time.minute();
//   int second = gps.time.second();
//   // int year = 2025;
//   // int month = 3;
//   // int day = 29;
//   // int hour = 1;  //+3 dilimi
//   // int minute = 4;
//   // int second = 32;
  
//   struct tm timeinfo = { 0 };      // Zaman yapısını başlat
//   timeinfo.tm_year = year - 1900;  // 1900'dan itibaren yıl
//   timeinfo.tm_mon = month - 1;     // Ayı 0-11 arası kabul eder
//   timeinfo.tm_mday = day;          // Gün
//   timeinfo.tm_hour = hour;         // Saat
//   timeinfo.tm_min = minute;        // Dakika
//   timeinfo.tm_sec = second;        // Saniye
//   return mktime(&timeinfo);  
// }

/*
float getLatitudeGPS() {
    static float lastLatitude = 0.0; // En son okunan değeri sakla

    while (gpsSerial.available() > 0) {
        gps.encode(gpsSerial.read()); // Yeni veriyi GPS'e işle

        if (gps.location.isUpdated()) {  // Yeni veri geldi mi?
            lastLatitude = gps.location.lat();  // Yeni veriyi kaydet
        }
    }

    return lastLatitude;  // Yeni veri yoksa en son okunanı döndür
}
*/