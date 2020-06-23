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
        }

        private void CheckConnection() => DisplayAlert("Connection demanded", "First connect to the RPi via Connect button.", "Ok");
        private void BttnConnect_Clicked(object sender, EventArgs e) => RefreshConnectionStatus();

        private void RefreshConnectionStatus()
        {
            switch (BluetoothController.ConnectToRPi())
            {
                case ConnectionState.BluetoothOFF:
                    LblConnectionStatus.Text = "Bluetooth is off!";
                    LblConnectionStatus.TextColor = Color.Gray;
                    break;
                case ConnectionState.Failed:
                    LblConnectionStatus.Text = "Connection failed with RPi.";
                    LblConnectionStatus.TextColor = Color.Red;
                    break;
                case ConnectionState.Connected:
                    LblConnectionStatus.Text = "Connection established with RPi.";
                    LblConnectionStatus.TextColor = Color.Green;
                    break;

            }
        }

        private void BttnSatData_Clicked(object sender, EventArgs e)
        {

            BluetoothController.SendDataToRPi("set_env terminal");
            Thread.Sleep(1000);
            BluetoothController.SendDataToRPi("dataON sat");
            Navigation.PushModalAsync(new SatDataPage());
        }

        private void BttnBaseData_Clicked(object sender, EventArgs e)
        {

            BluetoothController.SendDataToRPi("set_env terminal");
            Thread.Sleep(1000);
            BluetoothController.SendDataToRPi("dataON bs");
            Navigation.PushModalAsync(new BaseDataPage());
        }
    }
}
