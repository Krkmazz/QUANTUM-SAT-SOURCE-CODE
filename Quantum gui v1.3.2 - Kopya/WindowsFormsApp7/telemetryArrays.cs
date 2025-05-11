using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp7
{
    public class telemetryArrays
    {
        private SerialRead serialReader;

        // Constructor, serialPort'u parametre olarak alır
        public telemetryArrays(SerialRead reader)
        {
            serialReader = reader;  // SerialRead nesnesini başlatıyoruz readerle
        }


        // ARRAYLER VE STRİNG İFADELER BURADA MEVCUT FORM1.CS İÇERİSİNDE CAGRI YAPILACAK
        
        //List<string> Array_TelemetryCounts = new List<string>();
        public string Array_TelemetryCounts;
        //List<string> Array_SatelliteStatus = new List<string>();
        public string Array_SatelliteStatus;
        //List<string> Array_ARAS = new List<string>();
        public string Array_ARAS;
        //List<string> Array_DateAndTime = new List<string>();
        public string Array_DateAndTime;
        public List<string> Array_Pressure1 = new List<string>();      //
        public List<string> Array_Pressure2 = new List<string>();      //
        public List<string> Array_Altitude1 = new List<string>();      //
        public List<string> Array_Altitude2 = new List<string>();      //
        public List<string> Array_AltitudeDiff = new List<string>();   //
        public List<string> Array_DescentRate = new List<string>();    //
        public List<string> Array_Temperature = new List<string>();    //
        //List<string> Array_Voltage = new List<string>();
        public string Array_Voltage;
        //List<string> Array_Gps_Latitude = new List<string>();
        public string Array_Gps_Latitude;
        //List<string> Array_Gps_Longitude = new List<string>();
        public string Array_Gps_Longitude;
        //List<string> Array_Gps_Altitude = new List<string>();
        public string Array_Gps_Altitude;
        public List<string> Array_Imu_Roll = new List<string>();       //
        public List<string> Array_Imu_Pitch = new List<string>();      //
        public List<string> Array_Imu_Yaw = new List<string>();        // 
        public List<string> Array_Imu_RHRH = new List<string>();       // 
        // List<string> Array_IoTS1_data = new List<string>();
        public string Array_IoTS1_data;
        //List<string> Array_IoTS2_data = new List<string>();
        public string Array_IoTS2_data;
        //List<string> Array_TeamNo = new List<string>();
        public string Array_TeamNo;


        int indexCounter = 0;
        int pre_telemetryCount = 0;

        //RHRH TULU İLE CAGRILIYOR ŞU ANLIK 
        public void create_TelemetryArrays()
        {
            for (; indexCounter < serialReader.TelemetryDataList.Count; indexCounter++)
            {
                //Console.WriteLine(serialReader.TelemetryDataList[indexCounter][0]);
                Array_TelemetryCounts = serialReader.TelemetryDataList[indexCounter][0];
                Array_SatelliteStatus = serialReader.TelemetryDataList[indexCounter][1];
                Array_ARAS = serialReader.TelemetryDataList[indexCounter][2];
                Array_DateAndTime = serialReader.TelemetryDataList[indexCounter][3];
                Array_Pressure1.Add(serialReader.TelemetryDataList[indexCounter][4]);
                Array_Pressure2.Add(serialReader.TelemetryDataList[indexCounter][5]);
                Array_Altitude1.Add(serialReader.TelemetryDataList[indexCounter][6]);
                Array_Altitude2.Add(serialReader.TelemetryDataList[indexCounter][7]);
                Array_AltitudeDiff.Add(serialReader.TelemetryDataList[indexCounter][8]);
                Array_DescentRate.Add(serialReader.TelemetryDataList[indexCounter][9]);
                Array_Temperature.Add(serialReader.TelemetryDataList[indexCounter][10]);
                Array_Voltage = serialReader.TelemetryDataList[indexCounter][11];
                Array_Gps_Latitude = serialReader.TelemetryDataList[indexCounter][12];
                Array_Gps_Longitude = serialReader.TelemetryDataList[indexCounter][13];
                Array_Gps_Altitude = serialReader.TelemetryDataList[indexCounter][14];
                Array_Imu_Roll.Add(serialReader.TelemetryDataList[indexCounter][15]);
                Array_Imu_Pitch.Add(serialReader.TelemetryDataList[indexCounter][16]);
                Array_Imu_Yaw.Add(serialReader.TelemetryDataList[indexCounter][17]);
                Array_Imu_RHRH.Add(serialReader.TelemetryDataList[indexCounter][18]);
                Array_IoTS1_data = serialReader.TelemetryDataList[indexCounter][19];
                Array_IoTS2_data = serialReader.TelemetryDataList[indexCounter][20];
                Array_TeamNo = serialReader.TelemetryDataList[indexCounter][21];
            }
            foreach(var data in Array_Pressure1)
            {
               Console.WriteLine(data);
            }        
        }
        

    }
}

