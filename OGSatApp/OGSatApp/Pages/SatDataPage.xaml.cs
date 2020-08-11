using OGSatApp.Controllers;
using OGSatApp.Pages.Behaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private CancellationTokenSource _token;

        Task task;

        public SatDataPage()
        {
            InitializeComponent();

            Disappearing += SatDataPage_Disappearing;

            _token = new CancellationTokenSource();

            task = Task.Run(async () =>
            {
                _ = BluetoothController.SendQueryToRPiAsync(Query.DataSatellite);

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

                    if (_token.Token.IsCancellationRequested)
                    {
                        await BluetoothController.SendQueryToRPiAsync(Query.DataOFF);
                        await Task.Delay(1000);


                        while (BluetoothController._client.GetStream().DataAvailable)
                            BluetoothController.ReadDataFromRPi();




                        return;
                    }

                }
            });

        }

        private async void SatDataPage_Disappearing(object sender, EventArgs e)
        {
            _token.Cancel();


            Debug.WriteLine(task.IsCompleted);
        }

    }
}