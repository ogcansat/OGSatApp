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

            var data = await BPEJController.LoadRecordFromCodeWithHeader(CodeBPEJ.Climate, int.Parse(EntrBPEJcode.Text[0].ToString()));
            GUIAnimations.FillTableSection(TblSctnClimate, data.Item1, data.Item2);

            data = await BPEJController.LoadRecordFromCodeWithHeader(CodeBPEJ.Inclination, int.Parse(EntrBPEJcode.Text[2].ToString()));
            GUIAnimations.FillTableSection(TblSctnInclination, data.Item1, data.Item2);

            data = await BPEJController.LoadRecordFromCodeWithHeader(CodeBPEJ.SoilDepth, int.Parse(EntrBPEJcode.Text[3].ToString()));
            GUIAnimations.FillTableSection(TblSctnSoilDepth, data.Item1, data.Item2);

            var rows = await BPEJController.LoadBlockRecordsFromCodeWithoudHeader(CodeBPEJ.SoilUnit, int.Parse(EntrBPEJcode.Text.Substring(5, 2)));
            GUIAnimations.FillTableSection(TblSctnSoilUnit, new string[] { "Kód", "Popis" }, new string[] { rows[0], string.Join(Environment.NewLine, rows.Skip(1)) });

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

    }
}