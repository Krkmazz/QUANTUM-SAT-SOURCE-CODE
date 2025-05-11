
// MPU6050 I2C adresi
const int MPU6050_ADDR = 0x68;


// Kalibrasyon offset değerleri
float gyroX_offset = 0.0, gyroY_offset = 0.0, gyroZ_offset = 0.0;

// Gyroscope ve ivmeölçer verileri
int16_t gyroX_raw, gyroY_raw, gyroZ_raw;
int16_t accelX_raw, accelY_raw, accelZ_raw;
float gyroX, gyroY, gyroZ;
float accelX, accelY, accelZ;

// Açı değişkenleri
float angleX = 0.0, angleY = 0.0, angleZ = 0.0;

// Kalman filtre nesneleri
SimpleKalmanFilter kalmanX(2, 2, 0.01);
SimpleKalmanFilter kalmanY(2, 2, 0.01);
SimpleKalmanFilter kalmanZ(2, 2, 0.01);  // Yaw açısı için Kalman filtresi

// Zaman değişkenleri
unsigned long prevMicros = 0;
float deltaTime;

// Gyro hassasiyeti (LSB/dps)
const float LSB_SENSITIVITY = 65.5;  // ±500 dps için

//ROLL PİTCH YAW
float filteredRoll;
float filteredPitch;
float filteredYaw;

void setupIMU() {
  Wire.begin();
  Serial.println("ROLL,PITCH,YAW");

  // MPU6050'yi başlat
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(0x6B);
  Wire.write(0x00);
  Wire.endTransmission(true);

  // Gyro hassasiyetini ±500 dps olarak ayarla
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(0x1B);
  Wire.write(0x08);
  Wire.endTransmission(true);

  // İvmeölçer tam ölçek aralığını ayarla (±2g)
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(0x1C);
  Wire.write(0x00);
  Wire.endTransmission(true);

  delay(100);
  calibrateGyro();
  prevMicros = micros();
}

void getIMUData() {
  unsigned long currentMicros = micros();
  deltaTime = (currentMicros - prevMicros) / 1000000.0;
  prevMicros = currentMicros;

  readMPU6050();

  gyroX = ((float)gyroX_raw - gyroX_offset) / LSB_SENSITIVITY;
  gyroY = ((float)gyroY_raw - gyroY_offset) / LSB_SENSITIVITY;
  gyroZ = ((float)gyroZ_raw - gyroZ_offset) / LSB_SENSITIVITY;

  accelX = (float)accelX_raw / 16384.0;
  accelY = (float)accelY_raw / 16384.0;
  accelZ = (float)accelZ_raw / 16384.0;

  // İvmeölçer verileriyle Pitch ve Roll açılarını hesapla
  float accelPitch = atan2(accelY, sqrt(accelX * accelX + accelZ * accelZ)) * 180 / PI;
  float accelRoll = atan2(-accelX, sqrt(accelY * accelY + accelZ * accelZ)) * 180 / PI;

  // Kalman filtre uygulama
  filteredRoll = kalmanX.updateEstimate(accelRoll);
  filteredPitch = kalmanY.updateEstimate(accelPitch);

  // Yaw açısını jiroskop verisiyle hesapla ve Kalman filtresi ile stabilize et
  angleZ += gyroZ * deltaTime;
  if (angleZ > 360) angleZ = 0;
  if (angleZ < -360) angleZ = 0;
  filteredYaw = kalmanZ.updateEstimate(angleZ);


  //Sonuçları seri monitöre yazdır
  // Serial.print(filteredRoll);
  // Serial.print(",");
  // Serial.print(-filteredPitch);
  // Serial.print(",");
  // Serial.println(filteredYaw);  // Yaw'ı yazdır

  delay(10);
}

void readMPU6050() {
  Wire.beginTransmission(MPU6050_ADDR);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU6050_ADDR, 14, true);

  accelX_raw = (Wire.read() << 8 | Wire.read());
  accelY_raw = (Wire.read() << 8 | Wire.read());
  accelZ_raw = (Wire.read() << 8 | Wire.read());
  Wire.read();
  Wire.read();
  gyroX_raw = (Wire.read() << 8 | Wire.read());
  gyroY_raw = (Wire.read() << 8 | Wire.read());
  gyroZ_raw = (Wire.read() << 8 | Wire.read());
}

void calibrateGyro() {
  long gyroX_offset_sum = 0, gyroY_offset_sum = 0, gyroZ_offset_sum = 0;
  int numReadings = 500;

  for (int i = 0; i < numReadings; i++) {
    readMPU6050();
    gyroX_offset_sum += gyroX_raw;
    gyroY_offset_sum += gyroY_raw;
    gyroZ_offset_sum += gyroZ_raw;
    delay(2);
  }

  gyroX_offset = gyroX_offset_sum / numReadings;
  gyroY_offset = gyroY_offset_sum / numReadings;
  gyroZ_offset = gyroZ_offset_sum / numReadings;
}


int16_t getIMU_roll() {
  return (filteredRoll*100);
}
int16_t getIMU_pitch() {
  return (filteredPitch*100);
}
int16_t getIMU_yaw() {
  return (filteredYaw*100);
}
