using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Services.Models.Helpers
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

        public string GetName<T>() where T : class
        {
            return ApiNames.TryGetValue(typeof(T), out string val) ? val : throw new NotImplementedException();
        }

        public Type GetType(string name)
        {
            var res = ApiNames.FirstOrDefault(o => o.Value.Equals(name, StringComparison.OrdinalIgnoreCase)).Key;
            return res ?? throw new NotImplementedException();
        }

        public bool Compare<T>(string name) where T : class => typeof(T).Equals(GetType(name));
    }
}
