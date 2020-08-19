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

        /// <summary>
        /// Token for cancellation the listening of data from RPi
        /// </summary>
        private CancellationTokenSource _token;


        /// <summary>
        /// Initialize the page, create task for listening of data from RPi
        /// </summary>
        public BaseDataPage()
        {
            InitializeComponent();

            if (GUIAnimations.CheckConnection(this)) return;

            Disappearing += BaseDataPage_Disappearing;

            _token = new CancellationTokenSource();

            //Sends query for listening base station data.
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

                    //Cancellation operation
                    if (_token.Token.IsCancellationRequested)
                    {                       
                        await BluetoothController.ClearIncomeBuffer();
                        return;
                    }
                }

               
            });

            

        }


        /// <summary>
        /// Sends cancellation query to the RPi, cancel the listening task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BaseDataPage_Disappearing(object sender, EventArgs e)
        {
            await BluetoothController.SendQueryToRPiAsync(Query.DataOFF);
            _token.Cancel();
        }

    }
}