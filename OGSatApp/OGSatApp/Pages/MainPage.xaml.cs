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
            void UpdateConnectionString(string text, Color color) => Device.InvokeOnMainThreadAsync(() => { LblConnectionStatus.Text = text; LblConnectionStatus.TextColor = color; });

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

        private async void BttnSatData_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SatDataPage());
        }

        private async void BttnBaseData_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new BaseDataPage());
        }

        private async void BttnBPEJ_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new BPEJPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) => RefreshConnectionStatus();

        private async void ImgShutdown_Tapped(object sender, EventArgs e)
        {
            if (BluetoothController.ConnectionStatus != ConnectionState.Connected)
                return;

            string result = await DisplayActionSheet("Vyberte akci", "Zrušit", "", "Vypnout RPi", "Resetovat RPi", "Resetovat data monitoring");
            switch (result)
            {
                case "Vypnout RPi":
                    await BluetoothController.SendQueryToRPiAsync("shutdown");
                    RefreshConnectionStatus();
                    break;
                case "Resetovat RPi":
                    await BluetoothController.SendQueryToRPiAsync("reboot");
                    RefreshConnectionStatus();
                    await Task.Delay(10_000);
                    RefreshConnectionStatus();
                    break;
                case "Resetovat data monitoring":
                    await BluetoothController.SendQueryToRPiAsync("restartOG");
                    break;
            }
        }

        private async void BttnPlants_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushModalAsync(new PlantsPage());
        }
    }
}
