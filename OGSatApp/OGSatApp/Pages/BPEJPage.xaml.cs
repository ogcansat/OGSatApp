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

        private async void EntrBPEJcode_Completed(object sender, EventArgs e)
        {

            var data = await BPEJController.LoadBPEJDetailsAsync(CodeBPEJ.Climate, int.Parse(EntrBPEJcode.Text[0].ToString()));
            FillTableSection(TblSctnClimate, data.Item1, data.Item2);

            data = await BPEJController.LoadBPEJDetailsAsync(CodeBPEJ.Inclination, int.Parse(EntrBPEJcode.Text[2].ToString()));
            FillTableSection(TblSctnInclination, data.Item1, data.Item2);

            data = await BPEJController.LoadBPEJDetailsAsync(CodeBPEJ.SoilDepth, int.Parse(EntrBPEJcode.Text[3].ToString()));
            FillTableSection(TblSctnSoilDepth, data.Item1, data.Item2);

            data = await BPEJController.LoadBPEJDetailsAsync(CodeBPEJ.SoilUnit, int.Parse(EntrBPEJcode.Text.Substring(5, 2)), false);
            FillTableSection(TblSctnSoilUnit, new string[] { "Kód", "Popis"}, new string[] { data.Item2[0], string.Join(Environment.NewLine, data.Item2.Skip(1))});

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