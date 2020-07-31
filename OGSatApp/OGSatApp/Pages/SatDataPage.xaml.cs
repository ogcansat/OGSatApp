using OGSatApp.Controllers;
using OGSatApp.Pages.Behaviors;
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
                    Dispatcher.BeginInvokeOnMainThread(() => GUIAnimations.UpdateData(data, new Dictionary<string, Label>()
                    {
                        {"MessageID", LblMessage },
                        {"Temperature", LblTemp },
                        {"Humidity", LblHum },
                        {"Pressure", LblPress },
                        {"Altitude", LblAlt },
                        {"Longitude", LblLong },
                        {"Latitude", LblLat }
                    }, LblUpdateTime));

                }
            });


            _ = BluetoothController.SendQueryToRPiAsync(Query.DataSatellite);
            _listener.Start();

        }

        private async void SatDataPage_Disappearing(object sender, EventArgs e)
        {
            await BluetoothController.SendQueryToRPiAsync(Query.DataOFF);
            _listener.Abort();
        }

    }
}