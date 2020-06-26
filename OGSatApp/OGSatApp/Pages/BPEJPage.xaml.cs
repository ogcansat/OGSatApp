using InTheHand.Net.Sockets;
using OGSatApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace OGSatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BPEJPage : ContentPage
    {
        public BPEJPage()
        {
            InitializeComponent();
        }

        private void EntrBPEJcode_Completed(object sender, EventArgs e)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(BPEJPage)).Assembly;         
            
            
            Stream stream = assembly.GetManifestResourceStream("OGSatApp.FilesBPEJ.KlimatickyRegion.csv");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            string[] lines = text.Split('\n');
            FillTableSection(TblSctnClimate, lines[0].Split(';'), lines.First(x => x.StartsWith(EntrBPEJcode.Text[0].ToString())).Split(';'));


            Stream stream2 = assembly.GetManifestResourceStream("OGSatApp.FilesBPEJ.SklonitostExpozice.csv");
            text = "";
            using (var reader = new System.IO.StreamReader(stream2))
            {
                text = reader.ReadToEnd();
            }
            lines = text.Split('\n');
            FillTableSection(TblSctnInclination, lines[0].Split(';'), lines.First(x => x.StartsWith(EntrBPEJcode.Text[2].ToString())).Split(';'));

            stream2 = assembly.GetManifestResourceStream("OGSatApp.FilesBPEJ.HloubkaPudySkeletovitost.csv");
            text = "";
            using (var reader = new System.IO.StreamReader(stream2))
            {
                text = reader.ReadToEnd();
            }
            lines = text.Split('\n');
            FillTableSection(TblSctnSoilDepth, lines[0].Split(';'), lines.First(x => x.StartsWith(EntrBPEJcode.Text[3].ToString())).Split(';'));

            stream2 = assembly.GetManifestResourceStream("OGSatApp.FilesBPEJ.HlavniPudniJednotka.csv");
            text = "";
            using (var reader = new System.IO.StreamReader(stream2))
            {
                text = reader.ReadToEnd();
            }
            lines = text.Split('\n');
            int index = lines.IndexOf(x => x.StartsWith(EntrBPEJcode.Text.Substring(5, 2)));
            FillTableSection(TblSctnSoilUnit, new string[] { "Popis"}, new string[] { lines[index + 1] });
        }

        private void BttnGetBPEJ_Clicked(object sender, EventArgs e)
        {
            
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