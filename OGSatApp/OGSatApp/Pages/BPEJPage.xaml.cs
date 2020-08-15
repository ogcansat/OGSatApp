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
            // _ = ViewExtensions.RelRotateTo(ImgLoadingInfoBPEJ, 2800, 10000);

            PrgrssBrBPEJ.IsVisible = true;
            PrgrssBrBPEJ.Progress = 0;
            TblSctnPlants.Clear();
            TblSctnClimate.Clear();
            TblSctnInclination.Clear();
            TblSctnSoilDepth.Clear();
            TblSctnSoilUnit.Clear();
            BttnGetBPEJ.IsEnabled = false;

            _ = PrgrssBrBPEJ.ProgressTo(0.5, 23000, Easing.Linear);

            var data = await BluetoothController.GetDataFromRPiAsync("getBPEJ " + EntrBPEJcode.Text + " plants", 5000);
            GUIAnimations.FillTableSection(TblSctnPlants, data.Split('\n')[0].Split(';'), data.Split('\n')[1].Split(';'), async (s, a) =>
            {
                var plant = (((s as ViewCell).View as StackLayout).Children[0] as Label).Text;
                await Navigation.PushModalAsync(new PlantsPage(plant));
            });

            _ = PrgrssBrBPEJ.ProgressTo(0.6, 1250, Easing.Linear);

            data = await BluetoothController.GetDataFromRPiAsync("getBPEJ " + EntrBPEJcode.Text + " climate", 5000);
            GUIAnimations.FillTableSection(TblSctnClimate, data.Split('\n')[0].Split(';'), data.Split('\n')[1].Split(';'));

            _ = PrgrssBrBPEJ.ProgressTo(0.7, 1250, Easing.Linear);

            data = await BluetoothController.GetDataFromRPiAsync("getBPEJ " + EntrBPEJcode.Text + " inclination", 5000);
            GUIAnimations.FillTableSection(TblSctnInclination, data.Split('\n')[0].Split(';'), data.Split('\n')[1].Split(';'));

            _ = PrgrssBrBPEJ.ProgressTo(0.8, 1250, Easing.Linear);

            data = await BluetoothController.GetDataFromRPiAsync("getBPEJ " + EntrBPEJcode.Text + " soilDepth", 5000);
            GUIAnimations.FillTableSection(TblSctnSoilDepth, data.Split('\n')[0].Split(';'), data.Split('\n')[1].Split(';'));

            _ = PrgrssBrBPEJ.ProgressTo(0.9, 1250, Easing.Linear);

            data = await BluetoothController.GetDataFromRPiAsync("getBPEJ " + EntrBPEJcode.Text + " soilUnit", 5000);
            GUIAnimations.FillTableSection(TblSctnSoilUnit, data.Split('\n')[0].Split(';'), new string[] { String.Join("\n", data.Split('\n').Skip(1).ToArray()) });

            _ = PrgrssBrBPEJ.ProgressTo(1, 1250, Easing.Linear);

            PrgrssBrBPEJ.IsVisible = false;
            BttnGetBPEJ.IsEnabled = true;


            /*GUIAnimations.FillTableSection(TblSctnPlants, data.Split('\n')[0].Split(';'), data.Split('\n')[1].Split(';'));
            GUIAnimations.FillTableSection(TblSctnClimate, data.Split('\n')[2].Split(';'), data.Split('\n')[3].Split(';'));
            GUIAnimations.FillTableSection(TblSctnInclination, data.Split('\n')[4].Split(';'), data.Split('\n')[5].Split(';'));
            GUIAnimations.FillTableSection(TblSctnSoilDepth, data.Split('\n')[6].Split(';'), data.Split('\n')[7].Split(';'));
            GUIAnimations.FillTableSection(TblSctnSoilUnit, data.Split('\n')[8].Split(';'), data.Split('\n')[9].Split(';'));*/


            //ViewExtensions.CancelAnimations(ImgLoadingInfoBPEJ);


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
                EntrBPEJcode.Text = await BluetoothController.GetDataFromRPiAsync($"get_bpej {location.Longitude} {location.Latitude}", 1000, 1000);
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
      
    }
}