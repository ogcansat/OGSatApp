using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using OGSatApp.Pages.Behaviors;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
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
        public readonly static BluetoothClient _client = new BluetoothClient();

        public static async Task<string> ReadDataFromRPiAsync()
        {
            string data;
            byte[] bytes = new byte[1000];
            await _client.GetStream().ReadAsync(bytes, 0, bytes.Length);
            data = Encoding.ASCII.GetString(bytes).Trim('\0');
            return data;
        }



        #region ObsoleteMethods

        [Obsolete]
        public static ConnectionState ConnectToRPi()
        {
            if (CrossBluetoothLE.Current.State == BluetoothState.On)
            {
                //if (!Client.Connected)
                //{
                _client.Connect(_client.PairedDevices.ToList().FirstOrDefault(x => x.DeviceName == "raspberrypi").DeviceAddress, BluetoothService.SerialPort);
                return _client.Connected ? ConnectionState.Connected : ConnectionState.Failed;
                //}
                //return ConnectionState.Connected;
            }
            else
                return ConnectionState.BluetoothOFF;

        }

        [Obsolete("Use ReadDataFromRPiAsync method instead.")]
        public static string ReadDataFromRPi()
        {

            while (_client.Connected)
            {
                string data;
                byte[] bytes = new byte[500];
                _client.GetStream().Read(bytes, 0, bytes.Length);
                data = Encoding.ASCII.GetString(bytes).Trim('\0');
                if (!string.IsNullOrWhiteSpace(data))
                {
                    return data;
                }
            }
            return string.Empty;
        }


        [Obsolete]
        public static bool SendDataToRPi(string data)
        {
            if (_client.Connected)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                _client.GetStream().WriteAsync(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }

        #endregion
    }
}
