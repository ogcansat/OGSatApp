using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using OGSatApp.Pages.Behaviors;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OGSatApp.Misc;

namespace OGSatApp.Controllers
{

    /// <summary>
    /// Connection status Enum
    /// </summary>
    public enum ConnectionState
    {
        Unconnected,
        Connected,
        BluetoothOFF,
        Failed
    }


    public enum Query
    {
        [StringValue("dataON sat")]
        DataSatellite,
        [StringValue("dataON bs")]
        DataBaseStation,
        [StringValue("dataOFF")]
        DataOFF
    }


    /// <summary>
    /// Class for communication with connected device (RPi)
    /// </summary>
    public static class BluetoothController
    {

        /// <summary>
        /// Private field for connected device
        /// </summary>
        private readonly static BluetoothClient _client = new BluetoothClient();

        public static ConnectionState ConnectionStatus { get; private set; }

        /// <summary>
        /// Read data from connected device
        /// </summary>
        /// <param name="buffer">Size of buffer</param>
        /// <returns>Returns task that returns single line of string of readed data/returns>
        public static async Task<string> ReadDataFromRPiAsync(int buffer = 1000)
        {
            string data;
            byte[] bytes = new byte[buffer];
            await _client.GetStream().ReadAsync(bytes, 0, bytes.Length);
            data = Encoding.ASCII.GetString(bytes).Trim('\0');
            return data;
        }

        /// <summary>
        /// Sends query to connected device
        /// </summary>
        /// <param name="query">query to send</param>
        /// <returns>Returns task</returns>
        public static async Task SendQueryToRPiAsync(string query)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(query);
            await _client.GetStream().WriteAsync(bytes, 0, bytes.Length);
        }

        public static async Task SendQueryToRPiAsync(Query query)
        {
            await SendQueryToRPiAsync(query.GetStringValue());
        }



        /// <summary>
        /// Sends query to device and read data
        /// </summary>
        /// <param name="query">Query to send to the device</param>
        /// <param name="buffer">Size of buffer for reading message</param>
        /// <param name="delay">Delay in ms between Write query and Read data</param>
        /// <returns>Returns task with result of single line of readed data</returns>
        public static async Task<string> GetDataFromRPiAsync(string query, int buffer = 1000, int delay = 500)
        {
            await SendQueryToRPiAsync(query);
            await Task.Delay(delay);
            return await ReadDataFromRPiAsync(buffer);
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
