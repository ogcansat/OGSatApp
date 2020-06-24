﻿using OGSatApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BaseDataPage()
        {
            InitializeComponent();
            new Thread(() =>
            {
                while (true)
                {
                    string data = BluetoothController.ReadDataFromRPi();
                    Device.InvokeOnMainThreadAsync(() => UpdateData(data));
                }
            }).Start();
        }

        public void UpdateData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            string[] lines = data.Split('\n');
            foreach (var item in lines)
            {
                string[] values = item.Split(':');
                switch (values[0])
                {
                    case "Temperature":
                        LblTemp.Text = values[1].ToString() + " °C";
                        break;
                    case "Humidity":
                        LblHum.Text = values[1].ToString() + " %";
                        break;
                    case "Pressure":
                        LblPress.Text = values[1].ToString() + " hPa";
                        break;
                    case "Altitude":
                        LblAlt.Text = values[1].ToString() + " m";
                        break;
                    case "Light":
                        LblLight.Text = values[1].ToString() + " lux";
                        break;
                    case "SoilHum":
                        LblSoil.Text = values[1].ToString();
                        break;
                }
            }

            LblUpdateTime.Text = "Last updated: " + DateTime.Now.ToLongTimeString();

        }
    }
}