//using Bygdrift.DataLakeTools;
//using Bygdrift.Warehouse;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using Module.Refines;
//using Module.Services;
//using Module.Services.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Module.AppFunctions
//{
//    public class TimerTriggerOld
//    {
//        public AppBase<Settings> App { get; }

//        public WebService Service { get; }
//        public bool ForceApiFetch { get; }

//        public TimerTriggerOld(ILogger<TimerTriggerOld> logger, bool forceApiFetch = false)
//        {
//            App = new AppBase<Settings>(logger);
//            Service = new WebService(App);
//            ForceApiFetch = forceApiFetch;
//        }

////        [FunctionName(nameof(LoadData))]
////        public async Task LoadData([TimerTrigger("%ScheduleExpression%"
////#if DEBUG
////            ,RunOnStartup = true
////#endif
////            )] TimerInfo timerInfo)
////        {
////            App.Log.LogInformation("Start importing...");
////            if (string.IsNullOrEmpty(App.Settings.DaluxFMDataToFetch))
////                return;

////            foreach (var item in App.Settings.DaluxFMDataToFetch.ToLower().Replace(" ", string.Empty).Split(','))
////                await HandleDataAsync(item);
////        }

//        private async Task HandleDataAsync(string name)
//        {
//            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
//            if (name == "assets") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Asset>());
//            else if (name == "buildings") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Building>());
//            else if (name == "documents") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Document>());
//            else if (name == "estates") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Estate>());
//            else if (name == "locations") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Location>());
//            else if (name == "lots") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Lot>());
//            else if (name == "rooms") await GenericRefine.RefineAsync(App, await GetDataFromDataLakeOrApiAsync<Room>());
//            else App.Log.LogError($"In the appSetting 'DaluxFMDataToFetch', there are used an unknown name: '{name}'.");
//        }


//        /// <summary>If data are already saved in datalake, then use it from ther. Else fetch it from API</summary>
//        private async Task<List<T>> GetDataFromDataLakeOrApiAsync<T>() where T : class
//        {
//            var fileName = typeof(T).Name + ".json";
//            if (!ForceApiFetch && App.DataLake.GetJson("Raw", fileName, FolderStructure.DatePath, out List<T> data))
//                return data;
//            else
//            {
//                data = await Service.GetDataAsync<T>();
//                await App.DataLake.SaveObjectAsync(data, "Raw", fileName, FolderStructure.DatePath);
//                return data;
//            }
//        }
//    }
//}