using Bygdrift.DataLakeTools;
using Bygdrift.Warehouse;
using Module.Refines;
using Module.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.AppFunctions.Models
{
    public class DataCollection
    {
        public DataCollection(DataRoot dataRoot, string name)
        {
            DataRoot = dataRoot;
            Name = name;
        }

        public DataRoot DataRoot { get; }
        public string Name { get; }
        public bool IsLoaded { get; set; }
        public int? NextBookMark { get; set; }

        public async Task RunAsync()
        {
            if (IsLoaded)
                return;

            if (Name == "assets") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Asset>());
            else if (Name == "buildings") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Building>());
            else if (Name == "documents") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Document>());
            else if (Name == "estates") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Estate>());
            else if (Name == "locations") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Location>());
            else if (Name == "lots") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Lot>());
            else if (Name == "rooms") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<Room>());
            else if (Name == "workorders") await GenericRefine.RefineAsync(DataRoot.App, await GetDataFromDataLakeOrApiAsync<WorkOrder>());
            else DataRoot.App.Log.LogError($"In the appSetting 'DaluxFMDataToFetch', there are used an unknown name: '{Name}'.");
        }

        /// <summary>If data are already saved in datalake, then use it from ther. Else fetch it from API</summary>
        private async Task<List<T>> GetDataFromDataLakeOrApiAsync<T>() where T : class
        {
            var fileName = typeof(T).Name + ".json";
            if (NextBookMark == null && DataRoot.App.DataLake.GetJson("Raw", fileName, FolderStructure.DatePath, out List<T> data))
            {
                DataRoot.App.Log.LogInformation("Get {name} data. Got data from DataLake.", Name);
                IsLoaded = true;
                return data;
            }
            else
            {
                DataRoot.App.Log.LogInformation("Get {name} data. Loading data from WebService.", Name);

                var res = await DataRoot.Service.GetDataAsync<T>(7);

                if (NextBookMark == null && DataRoot.App.DataLake.GetJson("Raw", fileName, FolderStructure.DatePath, out List<T> val) && val != null)
                    res.Items.AddRange(val);

                await DataRoot.App.DataLake.SaveObjectAsync(res.Items, "Raw", fileName, FolderStructure.DatePath);

                NextBookMark = res.NextBookMark;

                if (res.NextBookMark == null)
                    IsLoaded = true;

                return res.Items;
            }
        }
    }
}