using Bygdrift.Warehouse;
using Module.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.AppFunctions.Models
{
    /// <summary>
    /// An exchange model between Orchestrator and ActivityTrigger.
    /// Designed to only keep few data, because Orchestrator saves it multiple places as json inside a datalake
    /// So data from webservice I have to store, is saved right place in DataLake.
    /// </summary>
    public class DataRoot
    {
        /// <param name="app"></param>
        /// <param name="itemsToFetch">Could be: "Assets, Buildings, Documents, Estates, Locations, Lots, Rooms, Workorders"</param>
        public DataRoot(AppBase<Settings> app, string itemsToFetch)
        {
            Collections = new();
            App = app;
            Service = new WebService(app);

            foreach (var name in itemsToFetch.ToLower().Replace(" ", string.Empty).Split(','))
                Collections.Add(new DataCollection(this, name));
        }

        public AppBase<Settings> App { get; }
        public WebService Service { get; }
        public List<DataCollection> Collections { get; set; }

        public bool AllLoaded
        {
            get { return !Collections.Any(o => !o.IsLoaded); }
        }

        public async Task<DataRoot> RunAsync()
        {
            var started = DateTime.Now;
            foreach (var item in Collections.Where(o => !o.IsLoaded))
            {
                await item.RunAsync();

                if (started.AddMinutes(6) < DateTime.Now)
                    break;

            }

            return this;
        }
    }
}