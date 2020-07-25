using InTheHand.Net.Sockets;
using OGSatApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using OGSatApp.Pages.Behaviors;
using System.Threading;
using System.Diagnostics;

namespace OGSatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BPEJPage : ContentPage
    {

        public BPEJPage()
        {
            InitializeComponent();

        }

        private async void EntrBPEJcode_Completed(object sender, EventArgs e)
        {
            _ = ViewExtensions.RelRotateTo(ImgLoadingInfoBPEJ, 2800, 10000);


            var data = await BluetoothController.GetDataFromRPiAsync("getBPEJ " + EntrBPEJcode.Text, 10000);

            FillTableSection(TblSctnClimate, data.Split('\n')[0].Split(';'), data.Split('\n')[1].Split(';'));
            FillTableSection(TblSctnInclination, data.Split('\n')[2].Split(';'), data.Split('\n')[3].Split(';'));
            FillTableSection(TblSctnSoilDepth, data.Split('\n')[4].Split(';'), data.Split('\n')[5].Split(';'));


            ViewExtensions.CancelAnimations(ImgLoadingInfoBPEJ);


            //data = await BPEJController.LoadBPEJDetailsAsync(CodeBPEJ.SoilUnit, int.Parse(EntrBPEJcode.Text.Substring(5, 2)), false);
            //FillTableSection(TblSctnSoilUnit, new string[] { "Kód", "Popis" }, new string[] { data.Item2[0], string.Join(Environment.NewLine, data.Item2.Skip(1)) });


        }

        private async void BttnGetBPEJ_Clicked(object sender, EventArgs e)
        {
            BttnGetBPEJ.IsEnabled = false;

            var token = GUIAnimations.DotLoadingAnimation(LblBPEJFinding, "Vyhodnocování", 7, 300);

            Location location = null;

            try
            {
                location = await Geolocation.GetLocationAsync();

                BluetoothController.SendDataToRPi($"get_bpej {location.Longitude} {location.Latitude}");
                await Task.Delay(1000);
                EntrBPEJcode.Text = await BluetoothController.ReadDataFromRPiAsync();

                EntrBPEJcode_Completed(null, null);
            }
            catch (FeatureNotEnabledException)
            {
                _ = DisplayAlert("Lokace není povolená", "Pro použití funkce si povolte lokaci v telefonu.", "OK");
            }
            catch(Exception ex)
            {
                _ = DisplayAlert("Debug - Exception", ex.Message, "OK");
            }

            token.Cancel();
            await Task.Delay(500);

            LblBPEJFinding.Text = location != null ? $"Vaše pozice | z. délka: {location.Longitude}, z. šířka: {location.Latitude}" : "Vaši pozici se nepodařilo lokalizovat.";

            BttnGetBPEJ.IsEnabled = true;
        }


        private void FillTableSection(TableSection section, string[] columns, string[] values)
        {
            section.Clear();

            for (int i = 0; i < columns.Length; i++)
            {
                var layout = new StackLayout() { Padding = 10 };
                layout.Children.Add(new Label() { Text = columns[i], TextColor = Color.Black });
                layout.Children.Add(new Label() { Text = values[i] });
                section.Add(new ViewCell() { View = layout });
            }
        }
    }
}