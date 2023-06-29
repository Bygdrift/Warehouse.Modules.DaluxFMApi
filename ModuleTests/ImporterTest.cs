using Bygdrift.CsvTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.Services.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// To use this test, fill out 'ModuleTests/appsettings.json'
/// You can upload data directly to Azure, by stting saveToServer = true
/// You can fetch data from webservice, by setting useDataFromService = true.
/// It takes some time to fetch data from webservices, so if you must test the code again and again, then download data to local files in: 'ModuleTests/Files/In/' and set useDataFromService=false.
/// Download the datafiles by using: ModuleTests.Service.WebServiceTest
/// </summary>

namespace ModuleTests
{
    [TestClass]
    public class ImporterTest
    {
        /// <summary>Path to project base</summary>
        public static readonly string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

        /// <summary>Get data from appSettings like Config["test"]</summary>
        public static readonly IConfigurationRoot Config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile("appsettings.local.json", optional: true).Build();

        //[TestMethod]
        //public void TestRunModule()
        //{
        //    var saveToServer = true;
        //    var useDataFromService = false;
        //    var loggerMock = new Mock<ILogger>();

        //    ImportResult res;
        //    if (useDataFromService)
        //    {
        //        var importer = new Module.Importer(Config, loggerMock.Object);
        //        var refines = importer.GetRefines();
        //        res = importer.Import(refines, saveToServer);
        //    }
        //    else
        //    {
        //        var data = new Dictionary<Type, List<object>>();
        //        GetDataFromTestFile<Asset>(ref data);
        //        GetDataFromTestFile<Building>(ref data);
        //        GetDataFromTestFile<Document>(ref data);
        //        GetDataFromTestFile<Estate>(ref data);
        //        GetDataFromTestFile<Location>(ref data);
        //        GetDataFromTestFile<Lot>(ref data);
        //        GetDataFromTestFile<Room>(ref data);
        //        var importer = new Module.Importer(Config, loggerMock.Object, useDataFromService, data);
        //        var refines = importer.GetRefines();
        //        res = importer.Import(refines, saveToServer);
        //    }

        //    var errors = res.Refines.Where(o => o.HasErrors);
        //    Assert.IsFalse(errors.Any());
        //    Assert.IsTrue(res.CommonDataModel != null);
        //    Assert.IsTrue(res.ImportLog != null);
        //    Assert.IsFalse(loggerMock.Invocations.Any(o => (LogLevel)o.Arguments[0] == LogLevel.Error));

        //    if (saveToServer)
        //        return;

        //    foreach (RefineBase item in res.Refines)
        //        item.Csv.ToFile(Path.Combine(BasePath, "Files", "Out", item.GetName() + ".csv"));

        //    File.WriteAllText(Path.Combine(BasePath, "Files", "Out", "model.json"), JsonConvert.SerializeObject(res.CommonDataModel, Formatting.Indented));

        //    res.ImportLog.Csv.ToFile(Path.Combine(BasePath, "Files", "Out", "importLog.csv"));
        //}

        //public void GetDataFromTestFile<T>(ref Dictionary<Type, List<object>> data)
        //{
        //    var fileName = typeof(T).Name + ".json";
        //    var path = Path.Combine(BasePath, "Files", "In", fileName);
        //    if (!File.Exists(path))
        //        throw new Exception("File is missing");

        //    var json = File.ReadAllText(path, Encoding.UTF8);
        //    var res = JsonConvert.DeserializeObject<List<T>>(json).Cast<object>().ToList();
        //    data.Add(typeof(T), res);
        //}

        //public static Csv GetCsv(string filename)
        //{
        //    var path = Path.Combine(BasePath, "Files", "Out", filename);
        //    return CsvImport.FromFile(path);
        //}
    }
}