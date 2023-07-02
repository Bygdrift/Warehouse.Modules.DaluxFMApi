using Bygdrift.DataLakeTools;
using Bygdrift.Warehouse;
using Container.Services;
using Container.Services.Models;

namespace Container
{
    public class Program
    {
        public static AppBase<Settings> App { get; } = new AppBase<Settings>();

        public static WebService Service { get; } = new WebService(App);

        public static async Task Main(string[] args)
        {
            if (string.IsNullOrEmpty(App.Settings.DaluxFMDataToFetch))
            {
                App.Log.LogInformation("No data was typed into setting: 'DaluxFMDataToFetch' Stopping.");
                return;
            }
            var names = App.Settings.DaluxFMDataToFetch.ToLower().Replace(" ", string.Empty).Split(',');
            var possiblNames = string.Join(',', new ApiName().ApiNames.Select(o => o.Value).ToArray());

            foreach (var name in names)
            {
                var apiName = new ApiName();
                if (apiName.Compare<Asset>(name)) await GetDataFromApiAsync<Asset>();
                if (apiName.Compare<AssetClassification>(name)) await GetDataFromApiAsync<AssetClassification>();
                else if (apiName.Compare<Building>(name)) await GetDataFromApiAsync<Building>();
                else if (apiName.Compare<Document>(name)) await GetDataFromApiAsync<Document>();
                else if (apiName.Compare<Estate>(name)) await GetDataFromApiAsync<Estate>();
                else if (apiName.Compare<Location>(name)) await GetDataFromApiAsync<Location>();
                else if (apiName.Compare<Lot>(name)) await GetDataFromApiAsync<Lot>();
                else if (apiName.Compare<Room>(name)) await GetDataFromApiAsync<Room>();
                else if (apiName.Compare<WorkOrder>(name)) await GetDataFromApiAsync<WorkOrder>();
                else App.Log.LogError("In the appSetting 'DaluxFMDataToFetch', there are used an unknown name: '{name}'. Possible names: '{possible}'.", name, possiblNames);
            }
        }

        private static async Task GetDataFromApiAsync<T>() where T : class
        {
            var name = typeof(T).Name;
            App.Log.LogInformation("Get {name} data. Loading data from WebService.", name);
            var res = await Service.GetDataAsync<T>();
            await App.DataLake.SaveObjectAsync(res?.Items, "Raw", name + ".json", FolderStructure.DatePath);
        }
    }
}