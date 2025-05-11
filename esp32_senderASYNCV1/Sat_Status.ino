/*  
    0 : Uçuşa Hazır (Roket Ateşlenmeden Önce)
    1 : Yükselme
    2: Model Uydu İniş
    3: Ayrılma
    4: Görev Yükü İniş
    5: Kurtarma (Görev Yükü’nün Yere Teması) 
*/

//YÜKSEKLİK KONTORLÜNÜ YAPAN FONKSİYON()    1-> YÜKSELİYOR 0->ALÇALIYOR 2->LİMİT İRTİFADAYIZ 3->İNİŞ TAMAMLANDI
int8_t altitudeRising() {
  int16_t sum = 0;
  for (int8_t i = 0; i < 5; i++) {
    sum += data.altitude1;        // BME sensöründen gelen yükseklik verisi 

    if(data.satellite_status == 2 && data.altitude1<=410 ){   // iniş aşamasındayken (statu 2) limit irtifaya gelinidğinde
      return 2;
    }

    delay(500);
  }

  int16_t averageAltitude = sum / 5;

  //ortalama irtifa 5 altındaysa indik sayıyoruz irtifa hesaplamasına güvenilmediği için şimdilik bir önlem
  if(averageAltitude < 5 ){
    return 3 ;
  }

  static int16_t prevAverageAltitude = 0;

  int8_t result = (averageAltitude > prevAverageAltitude) ? 1 : 0;

  prevAverageAltitude = averageAltitude;

  return result;
}


void satellite_status(void *pvParameters) {

  while (1) {
    int8_t* status = &data.satellite_status;  // pointer tanımı
    int8_t altitudeRising = altitudeRising();
    switch (*status) {
      case 0:
        if (altitudeRising == 1) {
          (*status)++;  //-> yüseklme aşamasına geçildi  statu 1 
        }
        break;

      case 1:
        if (altitudeRising == 0) {
          (*status)++;  //-> model uydu iniş aşamasına geçildi statu 2
        }
        break;


      case 2:
        if (altitudeRising == 2) {
          (*status)++;  // ayrılma aşamasına geçildiğini göster statu 3

          // AYRILMADAN SORUMLU FONKSİYONUN ÇALIŞTIRILMASI...

          // GÖREV YÜKÜ ÜZERİNDEKİ LORANIN KENDİSİNE GELEN VERİLERİ İŞLEMESİNİ SAĞLAYAN ANA ASENKRON FONKSİYONUN DEVREYE ALINMASI...

          // AYRILMANIN DOĞRULANMASI...
        }
        break;

      case 3:
         if (altitudeRising == 0) {
            (*status)++;   // Görev yükü iniş aşamasına geçildi. statu 4
         }
         break;

      case 4:
          if (altitudeRising == 3) {
            (*status)++;   // İniş tamamlandı. statu 5
          
            //sesli ikaz komutu çalıştırılsın. (buzzer code here)...

         }
         break;

      case 5:
          // Bu ana asenkron fonksiyonu devre dışı bırak...

    }
  }



  /*11.05.25 Yazılımında anne kodu var */
}