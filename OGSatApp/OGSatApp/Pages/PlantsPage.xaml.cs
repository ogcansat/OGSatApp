using OGSatApp.Controllers;
using OGSatApp.Pages.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace OGSatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlantsPage : ContentPage
    {

        private (string[], string[][]) _plants;

        public PlantsPage()
        {
            InitializeComponent();
            LoadPlants();
        }

        public PlantsPage(string plant)
        {
            InitializeComponent();
            LoadPlants();
            PckrPlants.SelectedItem = plant;
        }

        private async void LoadPlants()
        {
            var file = await BluetoothController.GetDataFromRPiAsync("getPlants", 50000);



            string[] lines = file.Split('\n');

            int index = 0;
            string[][] values = new string[lines.Length - 1][];
            lines.Skip(1).ForEach(x =>
            {
                values[index++] = x.Split(';');
            });

            _plants = (lines[0].Split(';'), values);



            string[] plantTitles = new string[_plants.Item2.Length];
            for (int i = 0; i < plantTitles.Length; i++)
            {
                plantTitles[i] = FormatPlantTitle(_plants.Item2[i][0], _plants.Item2[i][1], _plants.Item2[i][2]);
            }
            PckrPlants.ItemsSource = plantTitles;
        }

        private string FormatPlantTitle(string name, string breed, string type) => String.Format("{0} {1} ({2})", name, breed, type);

        private void PckrPlants_SelectedIndexChanged(object sender, EventArgs e)
        {
            TblSctnPlant.Clear();

            string[] columns = _plants.Item1;
            string[] values = _plants.Item2.First(x => string.Equals(FormatPlantTitle(x[0], x[1], x[2]), PckrPlants.SelectedItem.ToString()));

            GUIAnimations.FillTableSection(TblSctnPlant, columns, values);

        }
    }
}