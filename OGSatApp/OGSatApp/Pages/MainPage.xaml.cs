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
using System.Diagnostics;

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
            async void UpdateConnectionString(string text, Color color) => await Device.InvokeOnMainThreadAsync(() => { LblConnectionStatus.Text = text; LblConnectionStatus.TextColor = color; });

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
          
        }

        private void BttnSatData_Clicked(object sender, EventArgs e)
        {
           // BluetoothController.SendDataToRPi("dataON sat");
            Navigation.PushModalAsync(new SatDataPage());
        }

        private void BttnBaseData_Clicked(object sender, EventArgs e)
        {
            //BluetoothController.SendDataToRPi("dataON bs");
            Navigation.PushModalAsync(new BaseDataPage());
        }

        private void BttnBPEJ_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BPEJPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) => RefreshConnectionStatus();

        private async void ImgShutdown_Tapped(object sender, EventArgs e)
        {
            //if (!BluetoothController._client.Connected)
                //return;

            string result = await DisplayActionSheet("Vyberte akci", "Zrušit", "", "Vypnout RPi", "Resetovat RPi", "Resetovat data monitoring");
            switch (result)
            {
                case "Vypnout RPi":
                    BluetoothController.SendDataToRPi("shutdown");
                    RefreshConnectionStatus();
                    break;
                case "Resetovat RPi":
                    BluetoothController.SendDataToRPi("reboot");
                    RefreshConnectionStatus();
                    await Task.Delay(10_000);
                    RefreshConnectionStatus();
                    break;
                case "Resetovat data monitoring":
                    BluetoothController.SendDataToRPi("restartOG");
                    break;
            }
        }

        private void BttnPlants_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PlantsPage());
        }
    }
}
