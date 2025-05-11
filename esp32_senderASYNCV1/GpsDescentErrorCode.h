// 2. ANA ASENKRON FONKSİYON HEADER DOSYASI
#ifndef GpsDescentErrorCode_H
#define GpsDescentErrorCode_H

// Fonksiyonların declare edilmesi
void setErrorCodeAt(int index, char value) ;  //Diğer fonksiyonlarda error code oluşturulurken bu fonksiyona erişilsin.
void generateGpsDescentErrorCode(void *pvParameters);  // Fonksiyonun bildirimi

#endif