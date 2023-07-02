using Bygdrift.Warehouse;
using Container;
using Container.Services;
using Container.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace ContainerTests.Service
{
    [TestClass]
    public class WebServiceTest
    {
        /// <summary>Path to project base</summary>
        public static readonly string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

        private readonly WebService service;
        private readonly AppBase<Settings> app;

        public WebServiceTest()
        {
            var loggerMock = new Mock<ILogger>();
            app = new AppBase<Settings>(loggerMock.Object);
            service = new WebService(app);
        }

        [TestMethod]
        public async Task Slet()
        {
            Assert.AreEqual(false, new ApiName().TypeExists("lotss"));
            Assert.AreEqual(true, new ApiName().TypeExists("lots"));
        }

        [TestMethod]
        public async Task GetAssets()
        {
            var data = await service.GetDataAsync<Asset>(10);
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
        public async Task GetBuildings()
        {
            var data = await service.GetDataAsync<Building>(2);
            SaveToFile(data.Items);
        }

        [TestMethod]
        public async Task GetDocuments()
        {
            var data = await service.GetDataAsync<Document>(2);
            SaveToFile(data.Items);
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
        public async Task GetLocations()
        {
            var data = await service.GetDataAsync<Location>(2);
            SaveToFile(data.Items);
        }

        [TestMethod]
        public async Task GetLots()
        {
            var data = await service.GetDataAsync<Lot>(2);
            SaveToFile(data.Items);
        }

        [TestMethod]
        public async Task GetRooms()
        {
            var data = await service.GetDataAsync<Room>(2);
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

        private void SaveToFile<T>(List<T> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var fileName = typeof(T).Name + ".json";
            var filePath = Path.Combine(BasePath, "Files", "In", fileName);
            File.WriteAllText(filePath, json);
        }
    }
}
