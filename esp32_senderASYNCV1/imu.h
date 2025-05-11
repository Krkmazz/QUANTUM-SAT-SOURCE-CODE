// bme.h
#ifndef IMU.H
#define IMU_H

#include <Wire.h>
#include <SimpleKalmanFilter.h>



void setupIMU();
void getIMUData();
// void readMPU6050();
// void calibrateGyro();
int16_t getIMU_roll();
int16_t getIMU_pitch();
int16_t getIMU_yaw();

#endif