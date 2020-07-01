using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms.Internals;

namespace OGSatApp.Controllers
{
    public enum CodeBPEJ
    {
        Climate,
        Inclination,
        SoilDepth,
        SoilUnit
    }

    public static class BPEJController
    {
        private static Dictionary<CodeBPEJ, string> _fileBPEJ = new Dictionary<CodeBPEJ, string>()
        {
            {CodeBPEJ.Climate, "OGSatApp.FilesBPEJ.KlimatickyRegion.csv" },
            {CodeBPEJ.Inclination, "OGSatApp.FilesBPEJ.SklonitostExpozice.csv"},
            {CodeBPEJ.SoilDepth,  "OGSatApp.FilesBPEJ.HloubkaPudySkeletovitost.csv"},
            {CodeBPEJ.SoilUnit, "OGSatApp.FilesBPEJ.HlavniPudniJednotka.csv" }
        };
        private static Assembly _assembly = IntrospectionExtensions.GetTypeInfo(typeof(BPEJController)).Assembly;



        public static async Task<Tuple<string[], string[]>> LoadBPEJDetailsAsync(CodeBPEJ file, int code, bool row = true)
        {

            string[] lines;

            using (Stream stream = _assembly.GetManifestResourceStream(_fileBPEJ[file]))
            {
                string text = "";
                using (var reader = new StreamReader(stream))
                {
                    text = await reader.ReadToEndAsync();
                }
                lines = text.Split('\n');
            }

            if (row)
                return new Tuple<string[], string[]>(lines[0].Split(';'), lines.First(x => x.StartsWith(code.ToString())).Split(';'));
            else
            {
                int indexCode = lines.IndexOf(x => x.StartsWith(code.ToString()));
                int lastCode = lines.IndexOf(x => x.StartsWith((code + 1).ToString()));

                string[] data = lines.Skip(indexCode).Take((lastCode - indexCode)).ToArray();

                return new Tuple<string[], string[]>(null, data);
            }
            
        }


    }
}
