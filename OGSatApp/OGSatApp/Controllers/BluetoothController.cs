using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OGSatApp.Controllers
{
    public enum ConnectionState
    {
        Unconnected,
        Connected,
        BluetoothOFF,
        Failed     
    }

    public static class BluetoothController
    {    
        public static BluetoothClient Client { get; } = new BluetoothClient();

        public static ConnectionState ConnectToRPi()
        {
            if (!Client.Connected)
            {
                if (CrossBluetoothLE.Current.State == BluetoothState.On)
                {
                    Client.Connect(Client.PairedDevices.ToList().FirstOrDefault(x => x.DeviceName == "raspberrypi").DeviceAddress, BluetoothService.SerialPort);
                    return Client.Connected ? ConnectionState.Connected : ConnectionState.Failed;
                }
                else
                    return ConnectionState.BluetoothOFF;
            }
            return ConnectionState.Connected;
        }

        public static string ReadDataFromRPi()
        {
            while(Client.Connected)
            {
                string data;
                byte[] bytes = new byte[500];
                Client.GetStream().Read(bytes, 0, bytes.Length);
                data = Encoding.ASCII.GetString(bytes).Trim('\0');
                if (!string.IsNullOrWhiteSpace(data))
                {
                    return data;
                }
            }
            return string.Empty;
        }

        public static Task<string> ReadDataFromRPiAsync()
        {
            return Task.Factory.StartNew(ReadDataFromRPi, TaskCreationOptions.LongRunning);
        }

        public static bool SendDataToRPi(string data)
        {
            if (Client.Connected)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                Client.GetStream().WriteAsync(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }
    }
}
