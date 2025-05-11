
//Gönderilecek telemetri paketi ve uçuş yazılımı için bir data base

#pragma pack(push, 1)  // Yapının bellekte 1 byte hizalanmasını sağlar.
struct Signal {
  int16_t data_Count = 60;
  int8_t satellite_status = 0;
  char error_code[7] = "000000";  // 6 karakter + Null karakter (7)
  int32_t date_time = 0;          // Unix Timestamp (4 byte) // unsigned ?????????

  int16_t pressure1 = 7525;  // Float yerine int16_t (2 byte)
  int16_t pressure2 = 12654;
  int16_t altitude1 = 10012;
  int16_t altitude2 = 11012;
  int16_t altitude_diff = 12012;
  int16_t descent_rate = 13012;
  int16_t temperature = 14012;
  int16_t voltage = 15012;
  int32_t gps_latitude = 1601200;  // int32_t (4 byte)
  int32_t gps_longitude = 1701200;
  int16_t gps_altitude = 18012;

  int16_t imu_roll = 19012;
  int16_t imu_pitch = 20012;
  int16_t imu_yaw = 21012;

  char rhrh[5] = "9R5G";  // 4 karakter + Null karakter (5)
  int16_t IoTS1_data = 22012;
  int16_t IoTS2_data = 26012;

  char team_no[11] = "1140110564";  // 10 karakter + Null karakter (11)
};
#pragma pack(pop)

Signal data;  // data defination

