using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Module.Services;
using Module.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module
{
    public class Importer
    {
        private readonly IConfigurationRoot config;
        private readonly Dictionary<Type, List<object>> data;
        private readonly ILogger log;
        private readonly bool useDataFromService;

        //public Importer(IConfigurationRoot config, ILogger log, bool useDataFromService = true, Dictionary<Type, List<object>> data = null) : base(config, log)
        //{
        //    this.config = config;
        //    this.data = data;
        //    this.log = log;
        //    this.useDataFromService = useDataFromService;
        //}

        //public void MakRefines()
        //{
        //    var daluxFMDataToFetch = config["DaluxFMDataToFetch"];
        //    var service = new WebService(config["DaluxFMApiKey"], log);
        //    var refines = new List<RefineBase>();

        //    if (string.IsNullOrEmpty(daluxFMDataToFetch))
        //    {
        //        log.LogError("In the appSetting 'DaluxFMDataToFetch', there are not written any data to fetch, so no data will be loadet.");
        //        return default;
        //    }

        //    foreach (var item in daluxFMDataToFetch.ToLower().Replace(" ", string.Empty).Split(','))
        //    {
        //        if (item == "asset") refines.Add(new GenericRefine<Asset>(GetData<Asset>(service)));
        //        else if (item == "building") refines.Add(new GenericRefine<Building>(GetData<Building>(service)));
        //        else if (item == "document") refines.Add(new GenericRefine<Document>(GetData<Document>(service)));
        //        else if (item == "estate") refines.Add(new GenericRefine<Estate>(GetData<Estate>(service)));
        //        else if (item == "location") refines.Add(new GenericRefine<Location>(GetData<Location>(service)));
        //        else if (item == "lot") refines.Add(new GenericRefine<Lot>(GetData<Lot>(service)));
        //        else if (item == "room") refines.Add(new GenericRefine<Room>(GetData<Room>(service)));
        //        else log.LogError($"In the appSetting 'DaluxFMDataToFetch', there are used an unknown name: '{item}'.");
        //    }
        //    return refines;
        //}

        //private IEnumerable<T> GetData<T>(WebService service) where T : class
        //{
        //    if (data != null && data.TryGetValue(typeof(T), out List<object> value))
        //        return value.Cast<T>();
        //    else if (useDataFromService)
        //        return service.GetDataAsync<T>().Result;
        //    else
        //        return default;
        //}
    }
}