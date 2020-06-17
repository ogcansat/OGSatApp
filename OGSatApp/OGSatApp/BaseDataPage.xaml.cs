using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OGSatApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseDataPage : ContentPage
    {
        private string _data;

        public BaseDataPage()
        {
            InitializeComponent();
            GetData();
        }

        public async Task GetData()
        {
            while (true)
            {
                string data;
                do
                {
                    byte[] bytes = new byte[500];

                    await MainPage.Client.GetStream().ReadAsync(bytes, 0, bytes.Length);
                    data = Encoding.ASCII.GetString(bytes).Trim('\0');

                } while (data.Length < 10);
                _data = data;
                //LblData.Text = _data;
                UpdateData();
            }
        }

        public void UpdateData()
        {
            string[] lines = _data.Split('\n');
            foreach (var item in lines)
            {
                string[] values = item.Split(' ');
                switch (values[0])
                {
                    case "Temp:":
                        LblTemp.Text = values[1].ToString() + " °C";
                        break;
                    case "Hum:":
                        LblHum.Text = values[1].ToString() + " %";
                        break;
                    case "Press:":
                        LblPress.Text = values[1].ToString() + " hPa";
                        break;
                    case "Alt:":
                        LblAlt.Text = values[1].ToString() + " m";
                        break;
                    case "Light:":
                        LblLight.Text = values[1].ToString() + " lux";
                        break;
                    case "SoilHum:":
                        LblSoil.Text = values[1].ToString();
                        break;
                }
            }

            LblUpdateTime.Text = "Last updated: " + DateTime.Now.ToLongTimeString();

        }
    }
}