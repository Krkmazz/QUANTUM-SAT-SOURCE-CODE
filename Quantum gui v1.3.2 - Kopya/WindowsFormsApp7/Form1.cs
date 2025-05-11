using System;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using WindowsFormsApp7.Resources;



namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        private GPS gps;
        public static SerialPort serialPort; // Global erişilebilir SerialPort
        private SerialRead serialReader;
        private telemetryArrays telemetryArrays;
        


        public Form1()
        {
            InitializeComponent();
            gps = new GPS(webView21); // GPS sınıfını WebView2 ile başlat;  
 

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // panelMoveGps'e olayları ekle
            SetupDraggablePanel(panelMoveGps, gpsPanel);
            SetupDraggablePanel(panelGPSLabelHeader, gpsPanel);

            //TelemetryMovePanel
            SetupDraggablePanel(panelMoveTelePage, telemetriPanelPage);
            SetupDraggablePanel(telePanelPageHeader, telemetriPanelPage);


            EnableResizeForPanel(telemetriPanelPage);
            EnableResizeForPanel(gpsPanel);


           


        }
        private void dataStart_Click(object sender, EventArgs e)
        {
            serialPort = new SerialPort("COM8", 9600); // COM port ve baud rate
            serialPort.Open();
            MessageBox.Show("Seri port açıldı");

            // SerialRead sınıfına port nesnesini gönderiyoruz
            serialReader = new SerialRead(serialPort);
            telemetryArrays = new telemetryArrays(serialReader);
            serialReader.SetTelemetryArrays(telemetryArrays); // serialRead.cs içerisindde telemetryArray e erişim için.


        }


        private void dataStop_Click(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                MessageBox.Show("Seri port kapandı");   
            }
        }

        //GPS GÖSTERİM EKRANI 

      

        // Uyduya renk filtre verileri gönderme butonu 
        private void sendData_SatteliteBtn_Click(object sender, EventArgs e)
        {

            rhrh.SendMessageToESP(colorBox, serialPort);
            //telemetryArrays.create_TelemetryArrays();
            //Console.WriteLine(telemetryArrays.Array_Pressure1.Count);


        }


        //ARAYÜZ TEXTLERİNİ GÜNCELLENMESİ BÖLÜMÜ BURASI OLACAK 


        /*GRAFİK ÇİZDİRME BÖLÜMÜ BURASI OLACAK 
         * 
            --> Seri haberleşmeye başalt tusuna basıldıgında arrayler oluşmaya başlar !!!!!!!    
            --> Grafik çizdirilecek arraylere erişim örneği  -> telemetryArrays.Array_Pressure1

            --> telemetryArray.cs dosyası içerisinde arraylerin isimleri mevcut
            --> Graifk çizdirilmesi gerekenler
                    -telemetryArrays.Array_Pressure1
                    -telemetryArrays.Array_Pressure2                
                    -telemetryArrays.Array_Pressure1                    
                    -telemetryArrays.Array_Altitude1            --> Aynı birime sahip dizilerin grafiği aynı panel içerisine 
                    -telemetryArrays.Array_Alitutde2                çizilmeli örneğin basınc1 ve basınc2 grafiği
                    -telemetryArrays.Array_AltitudeDiff             Basınç Verileri içerisine çizilmeli
                    -telemetryArrays.Array_DescentRate
                    -telemetryArrays.Array_Temperature
        */
        
        //Pressure , Altitude , DescentRate ve Temp için ayrı grafikler oluşturuacak 
        private void drawPressure_Graph() { 
        
        } 





        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadLine(); // Gelen veriyi oku

            // UI thread'e güvenli şekilde mesaj kutusu göstermek için Invoke kullan
            this.Invoke(new Action(() =>
            {
                label94.Text = data;
            }));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }





        private void PageBtnClick(Panel panel)
        {
            panel.Visible = true;
            panel.Location = new Point(22, 69);
            panel.BringToFront();
        }


        private void gpsPageBtn_Click(object sender, EventArgs e)
        {
            PageBtnClick(gpsPanel);
            panelMain.Visible = false;
            telemetriPanelPage.Visible = false;
        }

        private void gpsPageCloseBtn_Click(object sender, EventArgs e)
        {
            gpsPanel.Visible = false;
            panelMain.Visible = true;
        }

        private void panelMainBtn_Click(object sender, EventArgs e)
        {
            panelMain.Visible = true;
            telemetriPanelPage.Visible = false;
            gpsPanel.Visible = false;
        }


        private void telePanelPageBtn_Click(object sender, EventArgs e)
        {
            PageBtnClick(telemetriPanelPage);
            panelMain.Visible = false;
            gpsPanel.Visible= false;
        }
        private void telePanelClosBtn_Click(object sender, EventArgs e)
        {
            telemetriPanelPage.Visible = false;
            panelMain.Visible = true;
        }

        private void curretTeleBoxHeader_DoubleClick(object sender, EventArgs e)
        {
            PageBtnClick(telemetriPanelPage);
            panelMain.Visible = false;
            gpsPanel.Visible = false;
        }


        //RESİZE PANEL PAGES
        private bool isResizing = false;
        private Point resizeStartPoint;
        private Size originalPanelSize;
        private const int resizeGripSize = 16;
        private Panel resizingPanel = null;
        private void EnableResizeForPanel(Panel panel)
        {
            panel.MouseDown += ResizePanel_MouseDown;
            panel.MouseMove += ResizePanel_MouseMove;
            panel.MouseUp += ResizePanel_MouseUp;
        }
        private void ResizePanel_MouseDown(object sender, MouseEventArgs e)
        {
            Panel panel = sender as Panel;
            if (IsInResizeArea(panel, e.Location))
            {
                isResizing = true;
                resizeStartPoint = e.Location;
                originalPanelSize = panel.Size;
                resizingPanel = panel;
                panel.Cursor = Cursors.SizeNWSE;
            }
        }

        private void ResizePanel_MouseMove(object sender, MouseEventArgs e)
        {
            Panel panel = sender as Panel;
            if (isResizing && resizingPanel == panel)
            {
                int widthChange = e.X - resizeStartPoint.X;
                int heightChange = e.Y - resizeStartPoint.Y;

                panel.Size = new Size(originalPanelSize.Width + widthChange, originalPanelSize.Height + heightChange);
            }
            else
            {
                if (IsInResizeArea(panel, e.Location))
                    panel.Cursor = Cursors.SizeNWSE;
                else
                    panel.Cursor = Cursors.Default;
            }
        }

        private void ResizePanel_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            resizingPanel = null;

            Panel panel = sender as Panel;
            panel.Cursor = Cursors.Default;
        }

        private bool IsInResizeArea(Panel panel, Point location)
        {
            return location.X >= panel.Width - resizeGripSize &&
                   location.Y >= panel.Height - resizeGripSize;
        }




        //TASINABİLİR PANEL PAGES
        // Olayları eklemek için bir fonksiyon
        private void SetupDraggablePanel(Control header, Panel panelToMove)
        {
            header.MouseDown += (s, e) => PanelHeader_MouseDown(s, e, panelToMove);
            header.MouseMove += PanelHeader_MouseMove;
            header.MouseUp += PanelHeader_MouseUp;
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragPanelPoint;
        private Panel currentDragPanel;
        private void PanelHeader_MouseDown(object sender, MouseEventArgs e, Panel targetPanel)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragPanelPoint = targetPanel.Location;
            currentDragPanel = targetPanel;
        }

        private void PanelHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && currentDragPanel != null)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                currentDragPanel.Location = Point.Add(dragPanelPoint, new Size(diff));
            }
        }

        private void PanelHeader_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            currentDragPanel = null;
        }
        private void panelMoveGps_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }


        private bool isRunning = false;
        private double latitude = 38.720493, longitude = 35.530986;




        private async void rjButton2_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                await gps.InitializeWebView2(latitude, longitude);
                await UpdateCoordinatesAsync();
            }
        }

    
        private async Task UpdateCoordinatesAsync()
        {
            while (isRunning)
            {
                latitude += 0.02;
                longitude += 0.02;

                await gps.UpdateMapLocation(latitude, longitude);
                await Task.Delay(1000);

                label123.Text = latitude.ToString();
                label121.Text = longitude.ToString();
            }
        }
      


    }
}
