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
using System.Data;
using System.Threading;

namespace OGSatApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static BluetoothClient Client;

        public MainPage()
        {
            InitializeComponent();
            Client = new BluetoothClient();

        }


        private void BttnConnect_Clicked(object sender, EventArgs e)
        {
            if (!Client.Connected)
            {
                if (CrossBluetoothLE.Current.State == BluetoothState.On)
                {
                    Client.Connect(Client.PairedDevices.ToList().FirstOrDefault(x => x.DeviceName == "raspberrypi").DeviceAddress, BluetoothService.SerialPort);
                    if (Client.Connected)
                    {
                        LblConnectionStatus.Text = "Connection established with RPi.";
                        LblConnectionStatus.TextColor = Color.Green;
                    }
                    else
                    {
                        LblConnectionStatus.Text = "Connection failed with RPi.";
                        LblConnectionStatus.TextColor = Color.Red;
                    }
                }
                else
                {
                    LblConnectionStatus.Text = "Bluetooth is off!";
                    LblConnectionStatus.TextColor = Color.Gray;

                }
            }
            
        }

        private void BttnSatData_Clicked(object sender, EventArgs e)
        {
            if (!Client.Connected)
            {
                DisplayAlert("Connection demanded", "First connect to the RPi via Connect button.", "Ok");
                return;
            }

            byte[] bytesOFF = Encoding.ASCII.GetBytes("set_env terminal");
            Client.GetStream().Write(bytesOFF, 0, bytesOFF.Length);

            Thread.Sleep(1000);

            byte[] bytesSatON = Encoding.ASCII.GetBytes("dataON sat");
            Client.GetStream().Write(bytesSatON, 0, bytesSatON.Length);
            Navigation.PushModalAsync(new SatDataPage());
        }

        private void BttnBaseData_Clicked(object sender, EventArgs e)
        {
            if (!Client.Connected)
            {
                DisplayAlert("Connection demanded", "First connect to the RPi via Connect button.", "Ok");
                return;
            }

            byte[] bytesOFF = Encoding.ASCII.GetBytes("set_env terminal");
            Client.GetStream().Write(bytesOFF, 0, bytesOFF.Length);

            Thread.Sleep(1000);

            byte[] bytesBsON = Encoding.ASCII.GetBytes("dataON bs");
            Client.GetStream().Write(bytesBsON, 0, bytesBsON.Length);
            Navigation.PushModalAsync(new BaseDataPage());
        }
    }
}
