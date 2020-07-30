using OGSatApp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OGSatApp.Controllers
{


    public static class PlantsController
    {

        private static (string[], string[][]) _plants = (null, null);

        public static (string[] Columns, string[][] Lines) Plants
        {
            get
            {
                return _plants;
            }
            set
            {
                _plants = value;
            }
        }

        public static string FormatPlantTitle(string name, string breed, string type) => String.Format("{0} {1} ({2})", name, breed, type);

        public static string FormatPlantTitle(string[] plantValue) => FormatPlantTitle(plantValue[0], plantValue[1], plantValue[2]);

        public static async Task LoadPlantsAsync()
        {

            var file = await BluetoothController.GetDataFromRPiAsync(Query.GetPlants.GetStringValue(), 50000,1000);

            string[] lines = file.Split('\n');

            int index = 0;
            string[][] values = new string[lines.Length - 1][];
            lines.Skip(1).ForEach(x =>
            {
                values[index++] = x.Split(';');
            });

            _plants = (lines[0].Split(';'), values);

        }
    }
}
