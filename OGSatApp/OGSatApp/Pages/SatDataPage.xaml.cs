using OGSatApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace OGSatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SatDataPage : ContentPage
    {

        private Thread _listener;

        public SatDataPage()
        {
            InitializeComponent();

            Disappearing += SatDataPage_Disappearing;

            _listener = new Thread(async () =>
            {
                while (true)
                {

                    string data = await BluetoothController.ReadDataFromRPiAsync();
                    Dispatcher.BeginInvokeOnMainThread(() => UpdateData(data));

                }
            });

            _listener.Start();

        }

        private void SatDataPage_Disappearing(object sender, EventArgs e)
        {
            BluetoothController.SendDataToRPi("dataOFF");
            _listener.Abort();
        }

        public void UpdateData(string data)
        {

            if (string.IsNullOrWhiteSpace(data))
                return;

            string[] lines = data.Split('\n');
            foreach (var item in lines)
            {
                string[] values = item.Split(':');
                switch (values[0])
                {
                    case "MessageID":
                        LblMessage.Text = values[1].ToString();
                        break;
                    case "Temperature":
                        LblTemp.Text = values[1].ToString() + " °C";
                        break;
                    case "Humidity":
                        LblHum.Text = values[1].ToString() + " %";
                        break;
                    case "Pressure":
                        LblPress.Text = values[1].ToString() + " hPa";
                        break;
                    case "Altitude":
                        LblAlt.Text = values[1].ToString() + " m";
                        break;
                    case "Longitude":
                        LblLong.Text = values[1].ToString();
                        break;
                    case "Latitude":
                        LblLat.Text = values[1].ToString();
                        break;
                }
            }

            LblUpdateTime.Text = "Poslední aktualizace: " + DateTime.Now.ToString("HH:mm:ss");

        }


    }
}