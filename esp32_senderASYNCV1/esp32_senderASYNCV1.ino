#include <Arduino.h>

#include "telemetry.h"
#include "sat_status.h"          // UYDU STATUSU BELİRLENMESİ --> 1.ANA ASENKRON FONKSİYON BURADA
#include "gpsdescenterrorcode.h" // GPS VE İNİŞ HIZI HATA KODU --> 2.ANA ASENKRON FONKSİYON BURADA
#include "imu.h"                 // IMU VERİLERİ ALINMASI
#include "gps.h"                 // GPS VERİLERİ ALINMASI


TaskHandle_t taskHandlePrintTelemetry = NULL;  // Başta hiçbir görevle ilişkilendirilmemiş
TaskHandle_t taskHandleTelemetryUpdate = NULL;
TaskHandle_t taskHandleSatStatus = NULL;       //1.ANA ASENKRON FONKSİYON HANDLE
TaskHandle_t taskHandleGpsDescentErrorCode = NULL;   //2.ANA ASENKRON FONKSİYON HANDLE
//TaskHandle_t sensorTaskHandle = NULL;

extern Signal data;  // Signal yapısından bir data oluşturduk exterb ile başka dosyada tanımlı olduğunu declare ettik


//Telemetri paketini sensörlerden ve modüllerle gelen verilerle güncelleme
void telemetry_update(void *pvParameters) {
  while (1) {
    getIMUData();

    data.imu_pitch = getIMU_pitch();
    data.imu_roll = getIMU_roll();
    data.imu_yaw = getIMU_yaw();

    //GPS DATALARI


    //BME680 DATALARI


    //Haberlşeme modülünden gelen datalar

    vTaskDelay(100 / portTICK_PERIOD_MS);
  }
}


void setup() {
  Serial.begin(115200);
  setupIMU();  //IMU başlatılması
  initGPS();   //GPS başlatılması


  // TELEMETRİ PAKETİ İÇERİĞİNİN OLUŞTURULMASI VE GÜNCELLENMESİ İÇİN DÖNGÜ BAŞLATILMASI
  xTaskCreatePinnedToCore(telemetry_update, "telemetryUpdate", 2048, NULL, 1, &taskHandleTelemetryUpdate, 0);  

  //TELEMETRİ PAKETİNDEN VERİ ALINARAK UYDU STATÜSÜ BELİRLENMESİ
  xTaskCreatePinnedToCore(satellite_status, "satStatus", 2048, NULL, 1, &taskHandleSatStatus, 0);  // -> 1.ANA ASENKRON FONKSİYON

  //TELEMETRİ PAKETİNDEN VERİ ALINARAK GPS VE İNİŞ HIZI HATA KODU OLUŞTURULMASI İÇİN DÖNGÜ BAŞLATILMASI
  xTaskCreatePinnedToCore(generateGpsDescentErrorCode, "GpsDescentErorrCodeTask", 2048, NULL, 1, &taskHandleGpsDescentErrorCode, 0); // -> 2.ANA ASENKRON FONKSİYON
}



void loop() {

  //TELEMETRİ PAKETİ İÇERİĞİNİ YAZDIRMAK İÇİN
  while (true) {
    // Signal verilerini yazdır
    Serial.println("----- Signal Data -----");
    Serial.print("Data Count: ");
    Serial.println(data.data_Count);
    Serial.print("Satellite Status: ");
    Serial.println(data.satellite_status);
    Serial.print("Error Code: ");
    Serial.println(data.error_code);
    Serial.print("Date/Time (Unix Timestamp): ");
    Serial.println(data.date_time);
    Serial.print("Pressure 1: ");
    Serial.println(data.pressure1);
    Serial.print("Pressure 2: ");
    Serial.println(data.pressure2);
    Serial.print("Altitude 1: ");
    Serial.println(data.altitude1);
    Serial.print("Altitude 2: ");
    Serial.println(data.altitude2);
    Serial.print("Altitude Difference: ");
    Serial.println(data.altitude_diff);
    Serial.print("Descent Rate: ");
    Serial.println(data.descent_rate);
    Serial.print("Temperature: ");
    Serial.println(data.temperature);
    Serial.print("Voltage: ");
    Serial.println(data.voltage);
    Serial.print("GPS Latitude: ");
    Serial.println(data.gps_latitude);
    Serial.print("GPS Longitude: ");
    Serial.println(data.gps_longitude);
    Serial.print("GPS Altitude: ");
    Serial.println(data.gps_altitude);
    Serial.print("IMU Roll: ");
    Serial.println(data.imu_roll);
    Serial.print("IMU Pitch: ");
    Serial.println(data.imu_pitch);
    Serial.print("IMU Yaw: ");
    Serial.println(data.imu_yaw);
    Serial.print("RHRH: ");
    Serial.println(data.rhrh);
    Serial.print("IoTS1 Data: ");
    Serial.println(data.IoTS1_data);
    Serial.print("IoTS2 Data: ");
    Serial.println(data.IoTS2_data);
    Serial.print("Team Number: ");
    Serial.println(data.team_no);
    Serial.println("------------------------");
    data.data_Count++;
    delay(1000);
    
  }
}

/*Made by KORKMAZ*/
