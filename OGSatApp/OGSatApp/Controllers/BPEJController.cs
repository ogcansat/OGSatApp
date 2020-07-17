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
    /// <summary>
    /// For accessing BPEJ files and their paths
    /// </summary>
    public enum CodeBPEJ
    {
        Climate,
        Inclination,
        SoilDepth,
        SoilUnit,
        Plants
    }

    public static class BPEJController
    {

        /// <summary>
        /// Dictionary of BPEJ files and their assembly path
        /// </summary>
        private static Dictionary<CodeBPEJ, string> _fileBPEJ = new Dictionary<CodeBPEJ, string>()
        {
            {CodeBPEJ.Climate, "OGSatApp.FilesBPEJ.KlimatickyRegion.csv" },
            {CodeBPEJ.Inclination, "OGSatApp.FilesBPEJ.SklonitostExpozice.csv"},
            {CodeBPEJ.SoilDepth,  "OGSatApp.FilesBPEJ.HloubkaPudySkeletovitost.csv"},
            {CodeBPEJ.SoilUnit, "OGSatApp.FilesBPEJ.HlavniPudniJednotka.csv" },
            {CodeBPEJ.Plants, "OGSatApp.FilesBPEJ.Rostliny.csv" }
        };

        /// <summary>
        /// Reference to the class's assembly containing BPEJ files
        /// </summary>
        private static Assembly _assembly = IntrospectionExtensions.GetTypeInfo(typeof(BPEJController)).Assembly;

        /// <summary>
        /// Loads entire file
        /// </summary>
        /// <param name="file">BPEJ file to lode</param>
        /// <returns></returns>
        private static async Task<string[]> LoadRows(CodeBPEJ file)
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

            return lines;
        }

        /// <summary>
        /// Loads single record from BPEJ file
        /// </summary>
        /// <param name="file">File with data of part of BPEJ code (CodeBPEJ.{Climate|Inclination|SoilDepth})</param>
        /// <param name="code">Part of BPEJ code to search through file (0~99)</param>
        /// <returns>Returns record with header according to the code</returns>
        public static async Task<(string[], string[])> LoadRecordFromCodeWithHeader(CodeBPEJ file, int code)
        {
            var lines = await LoadRows(file);
            return (lines[0].Split(';'), lines.First(x => x.StartsWith(code.ToString())).Split(';'));
        }

        /// <summary>
        /// Loads multiple rows from BPEJ file
        /// </summary>
        /// <param name="file">File with data of part of BPEJ code (CodeBPEJ.{SoilUnit})</param>
        /// <param name="code">Part of BPEJ code to search through file (0~99)</param>
        /// <returns>Returns rows of data according to the code</returns>
        public static async Task<string[]> LoadBlockRecordsFromCodeWithoudHeader(CodeBPEJ file, int code)
        {
            var lines = await LoadRows(file);
            int indexCode = lines.IndexOf(x => x.StartsWith(code.ToString()));
            int nextCode = lines.IndexOf(x => x.StartsWith((code + 1).ToString()));

            string[] data = lines.Skip(indexCode).Take((nextCode - indexCode)).ToArray();

            return data;
        }

        /// <summary>
        /// Loads all data from BPEJ file
        /// </summary>
        /// <param name="file">File with data of part of BPEJ code (CodeBPEJ.{Plants})</param>
        /// <returns>Returns header with rows containing separated values</returns>
        public static async Task<(string[], string[][])> LoadRecordsWithHeader(CodeBPEJ file)
        {

            string[] lines = await LoadRows(file);

            int index = 0;
            string[][] values = new string[lines.Length - 1][];
            lines.Skip(1).ForEach(x =>
            {
                values[index++] = x.Split(';');
            });

            return (lines[0].Split(';'), values);

        }


    }
}
