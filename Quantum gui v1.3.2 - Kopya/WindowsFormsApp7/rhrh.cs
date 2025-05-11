using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Linq;

namespace WindowsFormsApp7
{
    public class rhrh
    {
        public static void SendMessageToESP(TextBox colorBox, SerialPort serialPort)
        {
            
            string[] color_Codes = { "M", "F", "N", "R", "G", "B", "P", "Y", "C" };

            // Kullanıcının girdiği metni temizle (boşlukları kaldır, büyük harfe çevir)
            string input = colorBox.Text.Trim().ToUpper();
            
            // Harfleri tek tek kontrol et
            bool isValid = input.Where(char.IsLetter) // Harfleri filtrele
                              .All(c => color_Codes.Contains(c.ToString()));


            if ((input.Length == 4 || input.Length == 2) && isValid) // 4 haneli mi kontrol et
            {
                try
                {
                    if (serialPort.IsOpen) // Seri port açık mı kontrol et
                    {   
                        serialPort.WriteLine("rhrh" + input); // 4 haneli mesajı gönder

                        MessageBox.Show(input);
                    }
                    else
                    {
                        MessageBox.Show("Seri port açık değil! ");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Seri port hatası: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Hatalı Giriş");
                colorBox.Clear();
            }
        }
    }
}
