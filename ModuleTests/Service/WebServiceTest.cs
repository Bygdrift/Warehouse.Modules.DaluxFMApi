using Bygdrift.CsvTools;
using Bygdrift.Warehouse;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module;
using Module.Services;
using Module.Services.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleTests.Service
{
    [TestClass]
    public class WebServiceTest
    {
        private readonly WebService service;
        private readonly AppBase<Settings> app;

        public WebServiceTest()
        {
            var loggerMock = new Mock<ILogger>();
            app = new AppBase<Settings>(loggerMock.Object);
            service = new WebService(app);
        }

        [TestMethod]
        public async Task GetEstates()
        {
            var data = await service.GetDataAsync<Estate>(1);
            var errors = app.Log.GetErrorsAndCriticals();
            Assert.IsFalse(errors.Any());
            SaveToFile(data.Items);
        }

        [TestMethod]
        public async Task GetAssetClassifications()
        {
            var data = await service.GetDataAsync<AssetClassification>(1);
            var errors = app.Log.GetErrorsAndCriticals();
            Assert.IsFalse(errors.Any());
            SaveToFile(data.Items);
        }

        [TestMethod]
        public async Task GetWorkOrders()
        {
            var data = await service.GetDataAsync<WorkOrder>(2);
            var errors = app.Log.GetErrorsAndCriticals();
            Assert.IsFalse(errors.Any());
            SaveToFile(data.Items);
        }

        //[TestMethod]
        //public void GetBuildings()
        //{
        //    var data = service.GetDataAsync<Building>(2).Result;
        //    SaveToFile(data);
        //}

        //[TestMethod]
        //public void GetRooms()
        //{
        //    var data = service.GetDataAsync<Room>(2).Result;
        //    SaveToFile(data);
        //}

        //[TestMethod]
        //public void GetLocations()
        //{
        //    var data = service.GetDataAsync<Location>(2).Result;
        //    SaveToFile(data);
        //}

        //[TestMethod]
        //public void GetLots()
        //{
        //    var data = service.GetDataAsync<Lot>(2).Result;
        //    SaveToFile(data);
        //}

        [TestMethod]
        public async Task GetAssets()
        {
            var data = await service.GetDataAsync<Asset>(10);
            var errors = app.Log.GetErrorsAndCriticals();
            Assert.IsFalse(errors.Any());
            SaveToFile(data.Items);
        }


        //[TestMethod]
        //public void GetDocuments()
        //{
        //    var data = service.GetDataAsync<Document>(2).Result;
        //    SaveToFile(data);
        //}

        private void SaveToFile<T>(List<T> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var fileName = typeof(T).Name + ".json";
            var filePath = Path.Combine(ImporterTest.BasePath, "Files", "In", fileName);
            File.WriteAllText(filePath, json);
        }
    }
}
