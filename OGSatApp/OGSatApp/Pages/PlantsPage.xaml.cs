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

        public PlantsPage()
        {
            InitializeComponent();
            FillPlantsPicker();
        }

        public PlantsPage(string plant) : this()
        {
            PckrPlants.SelectedItem = plant;
        }

        private void FillPlantsPicker()
        {
            //PckrPlants.Items.Clear();
            PlantsController.Plants.Lines.ForEach(x => PckrPlants.Items.Add(PlantsController.FormatPlantTitle(x)));
        }     

        private void PckrPlants_SelectedIndexChanged(object sender, EventArgs e)
        {
            TblSctnPlant.Clear();

            string[] columns = PlantsController.Plants.Columns;
            string[] values = PlantsController.Plants.Lines.First(x => string.Equals(PlantsController.FormatPlantTitle(x), PckrPlants.SelectedItem.ToString()));

            GUIAnimations.FillTableSection(TblSctnPlant, columns, values);

        }
    }
}