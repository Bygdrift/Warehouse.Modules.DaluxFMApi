namespace Container.Services.Models
{
    public class ApiName
    {
        public Dictionary<Type, string> ApiNames { get; set; } = new Dictionary<Type, string>
        {
            { typeof(Asset), "assets"},
            { typeof(AssetClassification), "assetClassifications"},
            { typeof(Building), "buildings"},
            { typeof(Document), "documents"},
            { typeof(Estate), "estates"},
            { typeof(Location), "locations"},
            { typeof(Lot), "lots"},
            { typeof(Room), "rooms"},
            { typeof(WorkOrder), "workorders"}
        };

        public string GetName<T>() where T : class => ApiNames.TryGetValue(typeof(T), out var val) ? val : throw new NotImplementedException();

        public Type GetType(string name)
        {
            var res = ApiNames.FirstOrDefault(o => o.Value.Equals(name, StringComparison.OrdinalIgnoreCase)).Key;
            return res ?? throw new NotImplementedException();
        }

        public bool TypeExists(string name) => ApiNames.FirstOrDefault(o => o.Value.Equals(name, StringComparison.OrdinalIgnoreCase)).Key != null;

        public bool TypesExists(string[] names)
        {
            if (names == null)
                return true;

            foreach (var name in names)
                if (!TypeExists(name))
                    return false;

            return true;
            
        }

        public bool Compare<T>(string name) where T : class => typeof(T).Equals(GetType(name));
    }
}
