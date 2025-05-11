
// 2. ANA ASENKRON FONKSİYON KAYNAK DOSYASI

/*
1- Model uydu iniş hızının 12-14 m/s dışındaki değerlerde olması durumunda hata
kodu 1, renk kırmızı, belirlenen aralıkta olması durumunda hata kodu 0 renk ise
yeşil olmalıdır.
2- Görev yükü iniş hızının 6-8 m/s dışındaki değerlerde olması durumunda hata
kodu 1, renk kırmızı, belirlenen aralıkta olması durumunda hata kodu 0 renk ise
yeşil olmalıdır.
3- Taşıyıcı basınç verisi alınamaması durumunda hata kodu 1, renk kırmızı,
taşıyıcı basınç verisinin alınması durumunda hata kodu 0 renk ise yeşil
olmalıdır.
4- Görev yükü konum verisi alınamaması durumunda hata kodu 1, renk kırmızı,
görev yükü konum verisinin alınması durumunda hata kodu 0 renk ise yeşil
olmalıdır.
5- Ayrılmanın gerçekleşmemesi durumunda hata kodu 1, renk kırmızı, ayrılmanın
gerçekleşmesi durumunda hata kodu 0 renk ise yeşil olmalıdır.
6- Multi-spektral mekanik filtreleme sisteminin çalışmaması durumunda hata kodu
1, renk kırmızı, sistemin çalışması durumunda hata kodu 0 renk ise yeşil
olmalıdır.
*/

/*
    --> 2 numaralı ana asenkron fonksiyonumuz 
      Sürekli olarak iniş hızı ve gps konum kontrolü yapılır ve hata kodu oluşturulur
      Error kodu için gerekli olan 1,2 ve 4 numarnın kontrolü burarada yapılacak

      Diğer şartların kontrolü kendilerinden sorumlu fonksiyonlar altında

    --> data.error_code'a erişilerek indekler yardımıyla paket içerisinde dinamik bir error code oluşturulur
    //char error_code[7] = "000000";

*/


//Diğer fonksiyonlarda error code oluşturulurken bu fonksiyona erişilsin.
void setErrorCodeAt(int index, char value) {
  data.error_code[index] = value;
}

//-> 2 numaralı asenkron fonksiyon.
void generateGpsDescentErrorCode(void *pvParameters) {

  while(1){
    // GPS VERİLERİ ALINAMIYOR
    setErrorCodeAt(3, (data.gps_latitude == 0 || data.gps_longitude == 0) ? '1' : '0'); 

    int16_t descent = data.descent_rate;

     // Model uydu için hız kontrolü
    if (data.satelitte_status == 2) {
      setErrorCodeAt(0, !(12 < descent && descent < 14) ? '1' : '0');
    }
    // Görev yükü iniş hız kontrolü
    else if (data.satelitte_status == 4) {
      setErrorCodeAt(1, !(6 < descent && descent < 8) ? '1' : '0');
    }
    //İniş tamamlandıysa bu fonksiyonun çalışması durdurulsun 
    else if(data.satelitte_status == 5){
      vTaskDelete(taskHandleGpsDescentErrorCode);  // Görevi sil
      //vTaskSuspend(taskHandleGpsDescentErrorCode);  // Görevi durdur
      return;  // Fonksiyonu sonlandır
    }

    //!!!!!!!!!
    //vTaskDelayUntil kullanılması
     // 1 saniye bekle
    vTaskDelay(1000 / portTICK_PERIOD_MS);
  }
}
