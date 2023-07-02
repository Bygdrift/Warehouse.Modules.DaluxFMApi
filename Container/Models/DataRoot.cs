using Bygdrift.Warehouse;
using Container.Services;

namespace Container.Models
{
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

        public async Task<DataRoot> RunAsync()
        {
            foreach (var item in Collections.Where(o => !o.IsLoaded))
                await item.RunAsync();

            return this;
        }
    }
}