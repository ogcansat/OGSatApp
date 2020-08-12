using OGSatApp.Controllers;
using OGSatApp.Pages.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace OGSatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseDataPage : ContentPage
    {

        private CancellationTokenSource _token;

        public BaseDataPage()
        {
            InitializeComponent();

            Disappearing += BaseDataPage_Disappearing;

            _token = new CancellationTokenSource();

            _ = BluetoothController.SendQueryToRPiAsync(Query.DataBaseStation);

            Task.Run(async () =>
            {
                while (true)
                {
                    string data = await BluetoothController.ReadDataFromRPiAsync();
                    Dispatcher.BeginInvokeOnMainThread(() => GUIAnimations.UpdateData(data, new Dictionary<string, Label>()
                    {
                        {"Temperature", LblTemp },
                        {"Humidity", LblHum },
                        {"Pressure", LblPress },
                        {"Altitude", LblAlt },
                        {"Light", LblLight },
                        {"SoilHum", LblSoil }
                    }, LblUpdateTime));

                    if (_token.Token.IsCancellationRequested)
                    {                       
                        await BluetoothController.ClearIncomeBuffer();
                        return;
                    }
                }

               
            });

            

        }

        private async void BaseDataPage_Disappearing(object sender, EventArgs e)
        {
            await BluetoothController.SendQueryToRPiAsync(Query.DataOFF);
            _token.Cancel();
        }

    }
}