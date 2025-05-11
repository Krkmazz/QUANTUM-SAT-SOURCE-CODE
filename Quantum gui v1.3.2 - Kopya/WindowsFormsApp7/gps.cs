using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms; // WebView2 için doğru namespace

namespace WindowsFormsApp7
{
    public class GPS
    {
        private WebView2 webView;

        public GPS(WebView2 webViewControl)
        {
            webView = webViewControl;
        }

        public async Task UpdateMapLocation(double latitude, double longitude)
        {
            string script = $"updateMarker({latitude.ToString(CultureInfo.InvariantCulture)}, {longitude.ToString(CultureInfo.InvariantCulture)});";
            await webView.ExecuteScriptAsync(script);
        }

        public async Task InitializeWebView2(double latitude, double longitude)
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);

                string initialHtml = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
            <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
            <style>
                html, body, #map {{ height: 100%; margin: 0; padding: 0; }}
            </style>
        </head>
        <body>
            <div id='map'></div>
            <script>
                var map = L.map('map').setView([{latitude.ToString(CultureInfo.InvariantCulture)}, {longitude.ToString(CultureInfo.InvariantCulture)}], 35);
                L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
                    maxZoom: 19,
                    attribution: '© OpenStreetMap contributors'
                }}).addTo(map);

                var marker = L.marker([{latitude.ToString(CultureInfo.InvariantCulture)}, {longitude.ToString(CultureInfo.InvariantCulture)}]).addTo(map);

                function updateMarker(lat, lon) {{
                    marker.setLatLng([lat, lon]);
                    map.panTo([lat, lon]);
                }}
            </script>
        </body>
        </html>";

                webView.NavigateToString(initialHtml);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2 başlatılamadı: {ex.Message}");
            }
        }
    }
}
