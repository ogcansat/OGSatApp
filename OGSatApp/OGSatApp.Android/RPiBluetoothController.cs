using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;


namespace OGSatApp.Droid
{
    public static class RPiBluetoothController
    {

        private readonly static BluetoothAdapter _adapter = BluetoothAdapter.DefaultAdapter;

        public static BluetoothSocket RPiSocket { get; private set; }
      

        public static void CreateRPiSocket()
        {
            RPiSocket = _adapter.BondedDevices.FirstOrDefault(x => x.Name == "raspberrypi").CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString(""));           
        }
        

            
    }
}