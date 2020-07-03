using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using Xamarin.Forms.Internals;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Threading;
using OGSatApp.Controllers;

namespace OGSatApp.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            RefreshConnectionStatus();
        }

        private async void RefreshConnectionStatus()
        {
            _ = Task.Run(() => ViewExtensions.RelRotateTo(ImgRefreshConnection, 2800, 10000));

            UpdateConnectionString("Připojování...", Color.Gray);

            await Task.Run(() =>
            {
                switch (BluetoothController.ConnectToRPi())
                {
                    case ConnectionState.BluetoothOFF:
                        UpdateConnectionString("Bluetooth je vypnuto!", Color.Gray);
                        break;
                    case ConnectionState.Failed:
                        UpdateConnectionString("Připojení s RPi selhalo.", Color.Red);
                        break;
                    case ConnectionState.Connected:
                        UpdateConnectionString("Připojení s RPi navázano.", Color.Green);
                        break;
                } 
            });

            ViewExtensions.CancelAnimations(ImgRefreshConnection);

            void UpdateConnectionString(string text, Color color) => Dispatcher.BeginInvokeOnMainThread(() => { LblConnectionStatus.Text = text; LblConnectionStatus.TextColor = color; });
        }

        private void BttnSatData_Clicked(object sender, EventArgs e)
        {
            BluetoothController.SendDataToRPi("dataON sat");
            Navigation.PushModalAsync(new SatDataPage());
        }

        private void BttnBaseData_Clicked(object sender, EventArgs e)
        {
            BluetoothController.SendDataToRPi("dataON bs");
            Navigation.PushModalAsync(new BaseDataPage());
        }

        private void BttnBPEJ_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BPEJPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) => RefreshConnectionStatus();

    }
}
