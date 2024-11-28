using Bygdrift.Tools.CsvTool;
using Bygdrift.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module;
using Module.Refines;
using Module.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ModuleTests.AppFunctions
{
    /// <summary>
    /// Denne class er alene til Ad HOC tests
    /// </summary>
    [TestClass]
    public class DataTests
    {
        public static readonly string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
        private readonly AppBase<Settings> App;

        public DataTests()
        {
            App = new AppBase<Settings>();
        }

        [TestMethod]
        public async Task Call2()
        {
            var csv = new Csv().AddCsvFile(Path.Combine(BasePath, "Files", "In", "Document.csv"));
            var list = new List<int>();
            for (int i = csv.RowLimit.Min + 5; i < csv.RowLimit.Max; i++)
                list.Add(i);

            csv.RemoveRows(list.ToArray());
            App.Mssql.MergeCsv(csv, "Document2", "id", true, false);
            var j = App.Log.GetErrorsAndCriticals();
        }

        [TestMethod]
        public async Task Call3()
        {
            var file = File.ReadAllText(Path.Combine(BasePath, "Files", "In", "AssetClassification.json"));
            var json = JsonConvert.DeserializeObject<List<AssetClassification>>(file);
            await GenericRefine.RefineAsync<AssetClassification>(App, json, false);
        }

        [TestMethod]
        public async Task Call4()
        {
            var path = Path.Combine(BasePath, "Files", "In", "WorkOrder.json");
            var file = File.ReadAllText(path);
            var json = JsonConvert.DeserializeObject<List<WorkOrder>>(file);
            await GenericRefine.RefineAsync(App, json, false);
        }
    }
}
