using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp7
{
    public class SerialRead
    {
        private SerialPort serialPort;
        private const int StructSize = 65;      // Yapı boyutu
        private bool isProcessing = false;      // Veri işleme kontrol bayrağı
        private byte[] receivedData = new byte[StructSize];  // Veri tamponu
        private int dataIndex = 0;  // Veri dizisi işaretçisi

        //Telemetry Arraylerin oluşturulması için nesne oluşturuldu
        private telemetryArrays telemetryArrays;

        public SerialRead(SerialPort port)
        {
            serialPort = port;
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void SetTelemetryArrays(telemetryArrays telemetry)
        {
            telemetryArrays = telemetry;        
        }

        //TELEMETRY DATASININ TUTULDUGU LİSTE
        int counter = 0;
        public List<List<string>> TelemetryDataList = new List<List<string>>(); // Verileri saklamak için liste       
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Eğer bir işlem yapılırken başka veri gelirse, bu veriyi işlemeden bırak

            if (isProcessing) return;

            try
            {
                
                isProcessing = true; 

                // Seri port okuma
                int bytesToRead = serialPort.BytesToRead;
                byte[] tempData = new byte[bytesToRead];
                serialPort.Read(tempData, 0, bytesToRead);

                // Veriyi tamponda biriktiriyoruz
                foreach (var b in tempData)
                {
                    receivedData[dataIndex] = b;
                    dataIndex++;

                    // Tam veri alındığında işleme başla
                    if (dataIndex >= StructSize)
                    {
                        // Veri işleme
                        string data_Count = BitConverter.ToInt16(receivedData, 0).ToString();
                        string satellite_status = BitConverter.ToInt16(receivedData, 2).ToString();
                        string error_code = Encoding.UTF8.GetString(receivedData, 4, 7).TrimEnd('\0');  // Null karakterleri temizle
                        string date_time = BitConverter.ToInt16(receivedData, 11).ToString();
                        string pressure1 = BitConverter.ToInt16(receivedData, 13).ToString();
                        string pressure2 = BitConverter.ToInt16(receivedData, 15).ToString();
                        string altitude1 = BitConverter.ToInt16(receivedData, 17).ToString();
                        string altitude2 = BitConverter.ToInt16(receivedData, 19).ToString();
                        string altitude_diff = BitConverter.ToInt16(receivedData, 21).ToString();
                        string descent_rate = BitConverter.ToInt16(receivedData, 23).ToString();
                        string temperature = BitConverter.ToInt16(receivedData, 25).ToString();
                        string voltage = BitConverter.ToInt16(receivedData, 27).ToString();
                        string gps_latitude = BitConverter.ToInt32(receivedData, 29).ToString();
                        string gps_longitude = BitConverter.ToInt32(receivedData, 33).ToString();
                        string gps_altitude = BitConverter.ToInt16(receivedData, 37).ToString();
                        string imu_roll = BitConverter.ToInt16(receivedData, 39).ToString();
                        string imu_pitch = BitConverter.ToInt16(receivedData, 41).ToString();
                        string imu_yaw = BitConverter.ToInt16(receivedData, 43).ToString();
                        string rhrh = Encoding.UTF8.GetString(receivedData, 45, 5).TrimEnd('\0');
                        string IoTS1_data = BitConverter.ToInt16(receivedData, 50).ToString();
                        string IoTS2_data = BitConverter.ToInt16(receivedData, 52).ToString();
                        string team_no = Encoding.UTF8.GetString(receivedData, 54, 11).TrimEnd('\0');

                        // istersen debug için bunu yazdırabilirsin
                        //string output = $"sayac: {data_Count} °C , statu: {satellite_status} hPa , Error: {error_code} , Date Time: {date_time}" +
                        //    $" pre1: {pressure1} , pre2: {pressure2} , alti1: {altitude1} , alti2: {altitude2} , altiDiff: {altitude_diff}," +
                        //    $" Hız Farkı: {descent_rate} , Sıcaklık: {temperature} , Voltage: {voltage} , Gps_Lat: {gps_latitude} , Gps_Long: {gps_longitude}," +
                        //    $"Gps_Alt: {gps_altitude} , Roll: {imu_roll} , Pitch: {imu_pitch}, Yaw{imu_yaw} , RHRH: {rhrh} IoTS1: {IoTS1_data} , IoTS2: {IoTS2_data} , " +
                        //    $"teamno: {team_no}\n";
                        //Console.WriteLine(output);


                        List<string> current_telemetry = new List<string>
                        {
                            data_Count,              // 2 byte
                            satellite_status,        // 2 byte
                            error_code,              // 7 byte (char array)
                            date_time,               // 4 byte (Unix Timestamp)
                            pressure1,               // 2 byte
                            pressure2,               // 2 byte
                            altitude1,               // 2 byte
                            altitude2,               // 2 byte
                            altitude_diff,           // 2 byte
                            descent_rate,            // 2 byte
                            temperature,             // 2 byte
                            voltage,                 // 2 byte
                            gps_latitude,            // 4 byte
                            gps_longitude,           // 4 byte
                            gps_altitude,            // 2 byte
                            imu_roll,                // 2 byte
                            imu_pitch,               // 2 byte
                            imu_yaw,                 // 2 byte
                            rhrh,                    // 4 byte (char array)
                            IoTS1_data,              // 2 byte
                            IoTS2_data,              // 2 byte
                            team_no                  // 11 byte (char array)
                        };

                        // Veriyi diziye ekle

                        TelemetryDataList.Add(current_telemetry);

                        //telemetryArrayler oluşturulması için cağrı 
                        telemetryArrays.create_TelemetryArrays();

                        //current_telemetry dizisini birleştirerek string'e dönüştür
                        //string telemetryString = string.Join("\t", current_telemetry);
                        //Console.WriteLine(telemetryString); 

                        // Veriyi sıfırlıyoruz
                        dataIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);         
            }
            finally
            {
                isProcessing = false;

            }
        }
    }















    // Ve sana Allah, şanlı bir zaferle yardım eder.  -- Fetih 3 
    //Gerçekten, güçlükle beraber bir kolaylık vardır. Öyleyse, bir işi bitirince diğerine koyul.  -- İnşirah 6,7

}