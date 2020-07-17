using OGSatApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            Fill();
        }

        private async void Fill()
        {
            var items = await BPEJController.LoadPlantsDetailsAsync();
            string[] names = new string[items.Item2.Length];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = items.Item2[i][0];
            }
            PckrPlants.ItemsSource = names;
        }
    }
}